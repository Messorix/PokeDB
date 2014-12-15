using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Pokemon
{
    public class DataTemplateSelectorBase : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object inItem, DependencyObject inContainer)
        {
            DataRowView row = inItem as DataRowView;

            if (row != null)
            {
                if (row.DataView.Table.Columns.Contains("Status"))
                {
                    MainWindow w = GetMainWindow(inContainer);
                    return (DataTemplate)w.FindResource("StatusImage");
                }
            }
            return null;
        }

        public MainWindow GetMainWindow(DependencyObject inContainer)
        {
            DependencyObject c = inContainer;
            while (true)
            {
                DependencyObject p = VisualTreeHelper.GetParent(c);

                if (c is MainWindow)
                {
                    return c as MainWindow;
                }
                else
                {
                    c = p;
                }
            }
        }
    }
}
