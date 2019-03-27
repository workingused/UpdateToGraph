using EnvDTE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UpdateToGraphVSIX.Helper
{
    public static class ProjectItemWalker
    {
        public static ProjectItem GetFileByPath(Solution solution, string path)
        {
            ProjectItem projectItem = default(ProjectItem);
            foreach (Project project in solution.Projects)
            {
                if (project.ProjectItems != null)
                {
                    foreach (ProjectItem item in project.ProjectItems)
                    {
                        if (item.ProjectItems != null && item.ProjectItems.Count > 0)
                        {
                            Recursive(item);
                        }
                        else
                        {
                            if (item.FileNames[0].Equals(path))
                            {
                                projectItem = item;
                            }
                        }
                    }
                }
            }
            return projectItem;

            void Recursive(ProjectItem parentItem)
            {
                foreach (ProjectItem item in parentItem.ProjectItems)
                {
                    if (item.ProjectItems != null && item.ProjectItems.Count > 0)
                    {
                        Recursive(item);
                    }
                    else
                    {
                        if (item.FileNames[0].Equals(path))
                        {
                            projectItem = item;
                        }
                    }
                }
            }
        }
    }
}
