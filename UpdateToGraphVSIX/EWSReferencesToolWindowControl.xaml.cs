namespace UpdateToGraphVSIX
{
    using EnvDTE;
    using EnvDTE80;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.FindSymbols;
    using Microsoft.CodeAnalysis.MSBuild;
    using Microsoft.VisualStudio.Shell;
    using Microsoft.VisualStudio.Shell.Interop;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;
    using UpdateToGraphVSIX.Helper;

    /// <summary>
    /// Interaction logic for EWSReferencesToolWindowControl.
    /// </summary>
    public partial class EWSReferencesToolWindowControl : UserControl
    {
        private DTE2 dTE;
        /// <summary>
        /// Initializes a new instance of the <see cref="EWSReferencesToolWindowControl"/> class.
        /// </summary>
        public EWSReferencesToolWindowControl()
        {
            this.InitializeComponent();
            this.dTE = Package.GetGlobalService(typeof(EnvDTE._DTE)) as DTE2;
        }
        private void Button1_Click(object sender, RoutedEventArgs e)
        {
            SearchResult();
        }
        public void SearchResult()
        {
            //this.StackPanel.Children.Clear();
            if (!dTE.Solution.IsOpen)
            {
                MessageBox.Show("none solution openning,please open a solution first.");
                return;
            }
            if (string.IsNullOrEmpty(dTE.Solution.FullName)) return;

            List<SearchResultView> view = new List<SearchResultView>();
            IEnumerable<ReferencesResultModel> result = SymbolSearch(dTE.Solution.FullName);
            foreach (ReferencesResultModel r in result)
            {
                foreach (ReferenceLocation location in r.ReferenceLocation)
                {
                    view.Add(new SearchResultView(location)
                    {
                        Col = location.Location.GetLineSpan().StartLinePosition.Character,
                        Row = location.Location.GetLineSpan().StartLinePosition.Line + 1,
                        EwsApiName = r.EwsApiName,
                        FilePath = location.Location.SourceTree.FilePath,
                        GraphApiName = r.GraphApiName,
                        ProjectName = r.ProjectName
                    });
                }
            }
            this.DataGrid1.ItemsSource = view;
        }

        IEnumerable<ReferencesResultModel> SymbolSearch(string solutionPath)
        {
            HashSet<ReferencesResultModel> result = new HashSet<ReferencesResultModel>();
            var msWorkspace = MSBuildWorkspace.Create();
            var solution = msWorkspace.OpenSolutionAsync(solutionPath).Result;

            foreach (var project in solution.Projects)
            {
                Compilation compilation = project.GetCompilationAsync().Result;
                foreach (ApiMappingItem map in ExchangeMapToGraph.ExchangeMapToGraphList)
                {
                    var ss = compilation.GetTypeByMetadataName(map.Namespace + "." + map.ClassName);
                    if (ss != null)
                    {
                        if (!map.ClassName.Equals(map.MothedName))
                        {
                            var members = ss.GetMembers(map.MothedName);
                            foreach (var item in members)
                            {
                                result.Add(new ReferencesResultModel { EwsApiName = map.EWSApiName, Symbol = item, ProjectName = project.Name, GraphApiName = map.GraphApiName });
                            }
                        }
                        else
                        {
                            foreach (var item in ss.Constructors)
                            {
                                result.Add(new ReferencesResultModel { EwsApiName = map.EWSApiName, Symbol = item, ProjectName = project.Name, GraphApiName = map.GraphApiName });
                            }
                        }
                    }
                }
            }

            foreach (ReferencesResultModel model in result)
            {
                ISymbol symbol = model.Symbol;
                HashSet<ReferenceLocation> set = new HashSet<ReferenceLocation>();
                foreach (ReferencedSymbol refSymbol in SymbolFinder.FindReferencesAsync(symbol, solution).Result)
                {
                    foreach (var item in refSymbol.Locations.Where(s => s.Document.Project.Name.Equals(model.ProjectName)))
                    {
                        set.Add(item);
                    }
                }
                model.ReferenceLocation = set;
            }
            return result;
        }
        private void DataGrid1_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            DataGrid datagrid = sender as DataGrid;
            Point aP = e.GetPosition(datagrid);
            IInputElement obj = datagrid.InputHitTest(aP);
            DependencyObject target = obj as DependencyObject;
            if (target != null)
            {
                SearchResultView model = ((System.Windows.FrameworkElement)obj).DataContext as SearchResultView;
                if (model != null)
                {
                    HighlightWordTagger.keyword = model.EwsApiName.Substring(model.EwsApiName.LastIndexOf(".") > -1 ? model.EwsApiName.LastIndexOf(".") + 1 : 0);
                    EnvDTE.Window w = dTE.ItemOperations.OpenFile(model.FilePath);
                    ((EnvDTE.TextSelection)dTE.ActiveDocument.Selection).GotoLine(model.Row);
                }
            }
        }
    }
    public class SearchResultView
    {
        public string EwsApiName { get; set; }
        public string GraphApiName { get; set; }
        public string FilePath { get; set; }
        public int Col { get; set; }
        public int Row { get; set; }
        public string ProjectName { get; set; }

        private ReferenceLocation location { get; set; }
        public SearchResultView(ReferenceLocation location)
        {
            this.location = location;
        }
        public ReferenceLocation GetLocation()
        {
            return location;
        }
    }
    public class ReferencesResultModel
    {
        public string ProjectName { get; set; }
        public string EwsApiName { get; set; }
        public string GraphApiName { get; set; }
        public ISymbol Symbol { get; set; }
        public IEnumerable<ReferenceLocation> ReferenceLocation { get; set; }
    }
}