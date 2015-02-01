﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Profiler.Data;

namespace Profiler
{
	/// <summary>
	/// Interaction logic for SourceViewControl.xaml
	/// </summary>
	public partial class SourceViewControl : UserControl
	{
		public SourceViewControl()
		{
			this.InitializeComponent();
      Init();
    }

		private void DataGrid_AutoGeneratingColumn(object sender, System.Windows.Controls.DataGridAutoGeneratingColumnEventArgs e)
		{
      FrameDataTable.ApplyAutogeneratedColumnAttributes(e);
		}

    void Init()
    {
			SourceViewBase view = DataContext as SourceViewBase;
      if (view != null)
      {
        textEditor.Document.Text = view.Text;
        textEditor.CustomColumns = SourceColumns.Default;

        var lines = textEditor.Document.Lines;

        foreach (var item in view.Lines)
        {
          int index = item.Key - 1;
          if (index < lines.Count && lines[index].CustomData == null)
          {

            var data = new Dictionary<string, string>();
            lines[index].CustomData = data;
            data.Add(SourceColumns.TotalPercent.Name, String.Format("{0:0.##}%", item.Value.TotalPercent));
            data.Add(SourceColumns.Total.Name, String.Format("{0:0.###}", item.Value.Total));
						data.Add(SourceColumns.SelfPercent.Name, String.Format("{0:0.##}%", item.Value.SelfPercent));
						data.Add(SourceColumns.Self.Name, String.Format("{0:0.###}", item.Value.Self));
          }
        }

        if (lines.Count > 0)
        {
          lines[0].CustomData = new Dictionary<string, string>();
          foreach (var column in SourceColumns.Default)
            lines[0].CustomData.Add(column.Name, column.Name);
        }

        textEditor.Loaded += new RoutedEventHandler((object o, RoutedEventArgs e) => textEditor.ScrollTo(view.SourceFile.Line, 0) );
      }
    }

		private void UserControl_DataContextChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
		{
      Init();
		}

	}
}