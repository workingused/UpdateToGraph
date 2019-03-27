using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Adornments;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Operations;
using Microsoft.VisualStudio.Utilities;

namespace UpdateToGraphVSIX
{
    internal class TestQuickInfoSource : IQuickInfoSource
    {
        private TestQuickInfoSourceProvider m_provider;
        private ITextBuffer m_subjectBuffer;
        private IToolTipProviderFactory m_toolTipProviderFactory;
        private TextExtent m_extent;

        public TestQuickInfoSource(TestQuickInfoSourceProvider provider,
            ITextBuffer subjectBuffer,
            IToolTipProviderFactory toolTipProviderFactory)
        {
            m_provider = provider;
            m_subjectBuffer = subjectBuffer;
            m_toolTipProviderFactory = toolTipProviderFactory;
            //these are the method names and their descriptions
        }

        public void AugmentQuickInfoSession(IQuickInfoSession session, IList<object> qiContent, out ITrackingSpan applicableToSpan)
        {
            applicableToSpan = null;
            string shortMethodSignature = string.Empty;
            if (qiContent.Count > 0 && qiContent[0] is ContainerElement element)
            {
                shortMethodSignature = GetEWSMethodSignature(element);
            }

            ApiMappingItem mappingItem = ExchangeMapToGraph.MatchAccordinglyGraph(shortMethodSignature);
            if (mappingItem == null)
            {
                return;
            }
            // Map the trigger point down to our buffer.
            SnapshotPoint? subjectTriggerPoint = session.GetTriggerPoint(m_subjectBuffer.CurrentSnapshot);
            if (!subjectTriggerPoint.HasValue)
            {
                return;
            }

            ITextSnapshot currentSnapshot = subjectTriggerPoint.Value.Snapshot;
            SnapshotSpan querySpan = new SnapshotSpan(subjectTriggerPoint.Value, 0);

            //look for occurrences of our QuickInfo words in the span
            ITextStructureNavigator navigator = m_provider.NavigatorService.GetTextStructureNavigator(m_subjectBuffer);
            TextExtent extent = navigator.GetExtentOfWord(subjectTriggerPoint.Value);
            m_extent = extent;
            string searchText = extent.Span.GetText();
            //foreach (string key in m_dictionary.Keys)
            //{
            //    int foundIndex = searchText.IndexOf(key, StringComparison.CurrentCultureIgnoreCase);
            //    if (foundIndex > -1)
            //    {
            //        applicableToSpan = currentSnapshot.CreateTrackingSpan
            //            (
            //                                    //querySpan.Start.Add(foundIndex).Position, 9, SpanTrackingMode.EdgeInclusive
            //                                    extent.Span.Start + foundIndex, key.Length, SpanTrackingMode.EdgeInclusive
            //            );
            //        string value;
            //        m_dictionary.TryGetValue(key, out value);
            //        if (value != null)
            //        {
            //            this.toolTipProvider = m_toolTipProviderFactory.GetToolTipProvider(session.TextView);
            qiContent.Add(GetTitle());
            qiContent.Add(GetCodeSample(mappingItem.EWSMethodSignature));
            qiContent.Add(GetSampleProject(mappingItem.EWSApiName));
            qiContent.Add(GetToGraphExplorer());

            //        }
            //        else
            //            qiContent.Add("");

            //        return;
            //    }
            //}

            applicableToSpan = null;
        }

        private bool m_isDisposed;
        public void Dispose()
        {
            if (!m_isDisposed)
            {
                GC.SuppressFinalize(this);
                m_isDisposed = true;
            }
        }

        private string GetEWSMethodSignature(ContainerElement element)
        {
            string methodSignature = string.Empty;
            element = element?.Elements?.
               FirstOrDefault(m => m is ContainerElement) as ContainerElement;

            ClassifiedTextElement textElements = element?.Elements?.
                 FirstOrDefault(m => m is ClassifiedTextElement) as ClassifiedTextElement;


            string shortMethodSignature = textElements?.Runs?.Skip(2)?.Take(3)?.Aggregate("",
                (a, b) => a + b.Text);

            return shortMethodSignature;

            int indexOfNewLine = methodSignature.IndexOf(Environment.NewLine);
            if (indexOfNewLine > -1)
            {
                methodSignature = methodSignature.Remove(indexOfNewLine);
            }

            int indexOfSpace = methodSignature.IndexOf(' ') + 1;
            if (indexOfSpace != -1)
            {
                shortMethodSignature = methodSignature.Remove(0, indexOfSpace);
                int indexOfRightParenthesis = shortMethodSignature.IndexOf('(');
                if (indexOfRightParenthesis != -1)
                {
                    shortMethodSignature = shortMethodSignature.Remove(indexOfRightParenthesis);
                }

                shortMethodSignature = new String(shortMethodSignature.Where(s => (int)s != 8206).ToArray());
            }

            return shortMethodSignature;
        }

        private UIElement GetTitle()
        {
            Grid grid = new Grid();
            grid.Children.Add(new System.Windows.Controls.Image
            {
                HorizontalAlignment = HorizontalAlignment.Left,
                Height = 15,
                Width = 15,
                Source = Imaging.CreateBitmapSourceFromHBitmap(Properties.Resources.icon.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions())
            });
            TextBlock textBlock = new TextBlock { Margin = new Thickness(25, 0, 0, 0), Text = "Demo Api" };
            grid.Children.Add(textBlock);
            return grid;
        }
        private UIElement GetCodeSample(string EWSMethodSignature)
        {
            Regex regex = new Regex($"(?<=//{EWSMethodSignature})[\\s\\S]*?(?=//[A-Z])");

            string text = regex.Match(Properties.Resources.api).Value;
            text = text.Trim();
            var helpButton = new TextBox
            {
                Text = text,
                MaxHeight = 200,
                VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
            };
            helpButton.MouseDown += CreateSampleProject;
            return helpButton;
        }
        private UIElement GetSampleProject(string GraphApiName)
        {
            var helpButton = new LinkBlock
            {
                Text = "Create Graph sample project for me",
                Foreground = System.Windows.Media.Brushes.Blue,
                TextDecorations = TextDecorations.Underline,
                ApiName = GraphApiName,
                Cursor = System.Windows.Input.Cursors.Hand
            };
            helpButton.MouseDown += CreateSampleProject;
            return helpButton;
        }
        private UIElement GetToGraphExplorer()
        {
            var helpButton = new TextBlock
            {
                Text = "Try more on graph explorer",
                Foreground = System.Windows.Media.Brushes.Blue,
                TextDecorations = TextDecorations.Underline,
                Cursor = System.Windows.Input.Cursors.Hand
            };
            helpButton.MouseDown += (sender, e) =>
            {
                System.Diagnostics.Process.Start("https://developer.microsoft.com/en-us/graph/graph-explorer");
            };
            return helpButton;
        }
        private void CreateSampleProject(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            DTE2 dte2 = Package.GetGlobalService(typeof(DTE)) as DTE2;
            string SourceName = nameof(UpdateToGraphVSIX.Properties.Resources.UpdateToGraph);
            string extractPath = Path.GetDirectoryName(dte2.Solution.FullName);
            string destinationPath = Path.Combine(extractPath, SourceName);
            string projectFilePath = Path.Combine(destinationPath, "UpdateToGraph.csproj");
            if (!Directory.Exists(destinationPath))
            {
                byte[] array = new byte[UpdateToGraphVSIX.Properties.Resources.UpdateToGraph.Length];
                UpdateToGraphVSIX.Properties.Resources.UpdateToGraph.CopyTo(array, 0);
                string path = System.IO.Path.GetTempPath() + SourceName + ".zip";
                using (Stream outStream = File.Create(path))
                {
                    outStream.Write(array, 0, array.Length);
                }
                Directory.CreateDirectory(destinationPath);
                using (ZipArchive archive = ZipFile.OpenRead(path))
                {
                    archive.ExtractToDirectory(destinationPath);
                }
                File.Delete(path);
                dte2.Solution.AddFromFile(projectFilePath);
            }
            else if (File.Exists(projectFilePath))
            {
                bool exist = false;
                foreach (Project p in dte2.Solution.Projects)
                {
                    if (p.Name == SourceName)
                    {
                        exist = true;
                    }
                }
                if (!exist)
                {
                    dte2.Solution.AddFromFile(projectFilePath);
                }
            }
            LinkBlock link = (LinkBlock)sender;

            //string configFilePath = Path.Combine(destinationPath, "scripts", "graph-api-selected.js");
            //StreamReader streamReader = new StreamReader(configFilePath);
            //string content = streamReader.ReadToEnd();
            //string resultString = new Regex("(?<=').*?(?=')").Replace(content, link.ApiName);
            //streamReader.Close();
            //streamReader.Dispose();
            //StreamWriter streamWriter = new StreamWriter(configFilePath);
            //streamWriter.Write(resultString);
            //streamWriter.Flush();
            //streamWriter.Close();
            //streamWriter.Dispose();



            //const string TemplateName = "UpdateToGraph";

            //var DestinationPath = Path.Combine(Path.GetDirectoryName(dte2.Solution.FullName), TemplateName);
            //if (!Directory.Exists(DestinationPath))
            //{
            //    string root = Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase);
            //    string TemplatePath = Path.Combine(root, TemplateName);

            //    string projectFilePath = Path.Combine(DestinationPath, "UpdateToGraph.csproj");

            //    DirectoryInfo directoryInfo = new DirectoryInfo(TemplatePath);
            //    var newProject = Directory.CreateDirectory(Path.Combine(DestinationPath, TemplateName));
            //    foreach (var file in directoryInfo.GetFiles())
            //    {
            //        file.CopyTo(newProject.FullName);
            //    }
            //    dte2.Solution.AddFromFile(projectFilePath);
            //}

            //isHelpTipShow = true;
            //var content =
            //  new Border
            //  {
            //      Background = System.Windows.Media.Brushes.White,
            //      BorderBrush = System.Windows.Media.Brushes.LightGray,
            //      BorderThickness = new Thickness(1),
            //  };
            //var gird = new Grid { };
            //content.Child = gird;
            //gird.Children.Add(new Label { Content = "asdasdasd" });
            //this.toolTipProvider.ClearToolTip();
            //this.toolTipProvider.ShowToolTip(
            //    m_extent.Span.Snapshot.CreateTrackingSpan(m_extent.Span.Span, SpanTrackingMode.EdgeExclusive),
            //   content,
            //    PopupStyles.PositionClosest);
        }
    }

    [Export(typeof(IQuickInfoSourceProvider))]
    [Name("ToolTip QuickInfo Source")]
    [Order(After = "Default Quick Info Presenter")]
    [ContentType("text")]
    internal class TestQuickInfoSourceProvider : IQuickInfoSourceProvider
    {
        [Import]
        internal ITextStructureNavigatorSelectorService NavigatorService { get; set; }

        [Import]
        public IToolTipProviderFactory ToolTipProviderFactory { get; set; }

        [Import]
        internal ITextBufferFactoryService TextBufferFactoryService { get; set; }
        public IQuickInfoSource TryCreateQuickInfoSource(ITextBuffer textBuffer)
        {
            return new TestQuickInfoSource(this, textBuffer, ToolTipProviderFactory);
        }
    }
    internal class TestQuickInfoController : IIntellisenseController
    {
        private ITextView m_textView;
        private IList<ITextBuffer> m_subjectBuffers;
        private TestQuickInfoControllerProvider m_provider;
        private IQuickInfoSession m_session;
        internal TestQuickInfoController(ITextView textView, IList<ITextBuffer> subjectBuffers, TestQuickInfoControllerProvider provider)
        {
            m_textView = textView;
            m_subjectBuffers = subjectBuffers;
            m_provider = provider;

            m_textView.MouseHover += this.OnTextViewMouseHover;
        }
        private void OnTextViewMouseHover(object sender, MouseHoverEventArgs e)
        {
            //find the mouse position by mapping down to the subject buffer
            SnapshotPoint? point = m_textView.BufferGraph.MapDownToFirstMatch
                 (new SnapshotPoint(m_textView.TextSnapshot, e.Position),
                PointTrackingMode.Positive,
                snapshot => m_subjectBuffers.Contains(snapshot.TextBuffer),
                PositionAffinity.Predecessor);

            if (point != null)
            {
                ITrackingPoint triggerPoint = point.Value.Snapshot.CreateTrackingPoint(point.Value.Position,
                PointTrackingMode.Positive);
                if (!m_provider.QuickInfoBroker.IsQuickInfoActive(m_textView))
                {
                    m_session = m_provider.QuickInfoBroker.TriggerQuickInfo(m_textView, triggerPoint, true);
                }
            }
        }
        public void Detach(ITextView textView)
        {
            if (m_textView == textView)
            {
                m_textView.MouseHover -= this.OnTextViewMouseHover;
                m_textView = null;
            }
        }
        public void ConnectSubjectBuffer(ITextBuffer subjectBuffer)
        {
        }

        public void DisconnectSubjectBuffer(ITextBuffer subjectBuffer)
        {
        }
    }
    [Export(typeof(IIntellisenseControllerProvider))]
    [Name("ToolTip QuickInfo Controller")]
    [ContentType("text")]
    internal class TestQuickInfoControllerProvider : IIntellisenseControllerProvider
    {
        [Import]
        internal IQuickInfoBroker QuickInfoBroker { get; set; }
        public IIntellisenseController TryCreateIntellisenseController(ITextView textView, IList<ITextBuffer> subjectBuffers)
        {
            return new TestQuickInfoController(textView, subjectBuffers, this);
        }
    }

    internal class LinkBlock : TextBlock
    {
        public string ApiName { get; set; }
    }
}
