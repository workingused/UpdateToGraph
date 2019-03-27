using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Operations;
using Microsoft.VisualStudio.Text.Tagging;
using Microsoft.VisualStudio.Utilities;
using System.Windows.Media;
using Microsoft.VisualStudio.PlatformUI;
using System.Collections.ObjectModel;
using Microsoft.VisualStudio.Text.Formatting;

namespace UpdateToGraphVSIX
{
    internal class HighlightWordTag : TextMarkerTag
    {
        public HighlightWordTag() : base("MarkerFormatDefinition/HighlightWordFormatDefinition") { }

    }

    [Export(typeof(EditorFormatDefinition))]
    [Name("MarkerFormatDefinition/HighlightWordFormatDefinition")]
    [UserVisible(true)]
    internal class HighlightWordFormatDefinition : MarkerFormatDefinition
    {
        public HighlightWordFormatDefinition()
        {
            System.Drawing.Color select = VSColorTheme.GetThemedColor(ThemedDialogColors.SelectedItemActiveColorKey);
            System.Drawing.Color border = VSColorTheme.GetThemedColor(ThemedDialogColors.WindowBorderColorKey);

            this.BackgroundColor = Color.FromArgb(select.A, select.R, select.G, select.B);
            this.ForegroundColor = Color.FromArgb(border.A, border.R, border.G, border.B);
            this.DisplayName = "Highlight ExchangeAPI";
            this.ZOrder = 1;
        }
    }

    #region Implementing an ITagger
    internal class HighlightWordTagger : ITagger<HighlightWordTag>
    {
        public static string keyword = string.Empty;
        ITextView View { get; set; }
        ITextBuffer SourceBuffer { get; set; }
        ITextSearchService TextSearchService { get; set; }
        ITextStructureNavigator TextStructureNavigator { get; set; }
        NormalizedSnapshotSpanCollection WordSpans { get; set; }
        SnapshotPoint RequestedPoint { get; set; }
        object updateLock = new object();
        public event EventHandler<SnapshotSpanEventArgs> TagsChanged;

        public HighlightWordTagger(ITextView view, ITextBuffer sourceBuffer, ITextSearchService textSearchService,
ITextStructureNavigator textStructureNavigator)
        {
            this.View = view;
            this.SourceBuffer = sourceBuffer;
            this.TextSearchService = textSearchService;
            this.TextStructureNavigator = textStructureNavigator;
            this.WordSpans = new NormalizedSnapshotSpanCollection();
            this.View.Caret.PositionChanged += CaretPositionChanged;
        }

        void CaretPositionChanged(object sender, CaretPositionChangedEventArgs e)
        {
            UpdateAtCaretPosition(e.NewPosition);
        }

        void UpdateAtCaretPosition(CaretPosition caretPosition)
        {
            SnapshotPoint? point = caretPosition.Point.GetPoint(SourceBuffer, caretPosition.Affinity);

            if (!point.HasValue)
                return;

            RequestedPoint = point.Value;
            UpdateWordAdornments(RequestedPoint);
        }

        void UpdateWordAdornments(SnapshotPoint RequestPoint)
        {
            SnapshotPoint currentRequest = RequestPoint;
            List<SnapshotSpan> wordSpans = new List<SnapshotSpan>();
            List<SnapshotSpan> apiSpans = new List<SnapshotSpan>();
            string apiStartSign = ".";
            char commentLineSign = '/';

            string findWord = keyword.Contains('.') ? keyword : apiStartSign + keyword;
            FindData findData = new FindData(findWord, currentRequest.Snapshot);
            findData.FindOptions = FindOptions.WholeWord | FindOptions.MatchCase;
            SnapshotSpan? span = TextSearchService.FindNext(RequestPoint.Position, false, findData);
            if (span.HasValue)
            {
                wordSpans.Add(span.Value);
            }

            // Exclude matches in comment line and that are not follow “.”
            wordSpans.ForEach(x =>
            {
                ITextViewLine line = this.View.GetTextViewLineContainingBufferPosition(x.Start);
                if (!(line.Extent.GetText().Trim().StartsWith(commentLineSign.ToString())))
                {
                    if (keyword.Contains('.'))
                    {
                        apiSpans.Add(new SnapshotSpan(x.Snapshot, x.Start, x.Length));
                    }
                    else
                    {
                        apiSpans.Add(new SnapshotSpan(x.Snapshot, x.Start + 1, x.Length - 1));
                    }
                }
            });

            keyword = string.Empty;
            //If another change hasn't happened, do a real update
            if (currentRequest == RequestedPoint)
                SynchronousUpdate(currentRequest, new NormalizedSnapshotSpanCollection(apiSpans));
        }

        static bool WordExtentIsValid(SnapshotPoint currentRequest, TextExtent word)
        {
            return word.IsSignificant
                && currentRequest.Snapshot.GetText(word.Span).Any(c => char.IsLetter(c));
        }

        void SynchronousUpdate(SnapshotPoint currentRequest, NormalizedSnapshotSpanCollection newSpans)
        {
            lock (updateLock)
            {
                if (currentRequest != RequestedPoint)
                    return;

                WordSpans = newSpans;

                var tempEvent = TagsChanged;
                if (tempEvent != null)
                    tempEvent(this, new SnapshotSpanEventArgs(new SnapshotSpan(SourceBuffer.CurrentSnapshot, 0, SourceBuffer.CurrentSnapshot.Length)));
            }
        }

        public IEnumerable<ITagSpan<HighlightWordTag>> GetTags(NormalizedSnapshotSpanCollection spans)
        {
            // Hold on to a "snapshot" of the word spans, so that we maintain the same collection throughout  
            NormalizedSnapshotSpanCollection wordSpans = WordSpans;

            if (spans.Count == 0 || wordSpans.Count == 0)
                yield break;

            // If the requested snapshot isn't the same as the one our words are on, translate our spans to the expected snapshot   
            if (spans[0].Snapshot != wordSpans[0].Snapshot)
            {
                wordSpans = new NormalizedSnapshotSpanCollection(
                    wordSpans.Select(span => span.TranslateTo(spans[0].Snapshot, SpanTrackingMode.EdgeExclusive)));
            }

            // yield all the APIs in the file   
            foreach (SnapshotSpan span in NormalizedSnapshotSpanCollection.Overlap(spans, wordSpans))
            {
                yield return new TagSpan<HighlightWordTag>(span, new HighlightWordTag());
            }
        }
    }
    #endregion

    #region Creating a Tagger Provider
    [Export(typeof(IViewTaggerProvider))]
    [ContentType("text")]
    [TagType(typeof(TextMarkerTag))]
    internal class HighlightWordTaggerProvider : IViewTaggerProvider
    {
        [Import]
        internal ITextSearchService TextSearchService { get; set; }

        [Import]
        internal ITextStructureNavigatorSelectorService TextStructureNavigatorSelector { get; set; }

        public ITagger<T> CreateTagger<T>(ITextView textView, ITextBuffer buffer) where T : ITag
        {
            //provide highlighting only on the top buffer   
            if (textView.TextBuffer != buffer)
                return null;

            ITextStructureNavigator textStructureNavigator =
                TextStructureNavigatorSelector.GetTextStructureNavigator(buffer);

            return new HighlightWordTagger(textView, buffer, TextSearchService, textStructureNavigator) as ITagger<T>;
        }

    }
    #endregion
}
