using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using System.ComponentModel;
using System.Management;
using WordParser.Models;
using System.Windows;
using System.Data.Entity;
using Prism.Commands;
using Microsoft.Win32;
using Prism.Mvvm;

namespace WordParser
{
    class ViewModel : BindableBase
    {
        readonly RAMCounter _ramCounter = new RAMCounter();

        public ViewModel()
        {
            FilePathCommand = new DelegateCommand(() =>
            {
                OpenFileDialog fileDialog = new OpenFileDialog();
                fileDialog.Filter = "HTML или текст |*.txt;*.html";
                if (fileDialog.ShowDialog() == false)
                return;
                FilePath = fileDialog.FileName;
            });

            BeginExecutionCommand = new DelegateCommand(() =>
            {
                HtmlFileParser htmlFileParser = new HtmlFileParser(FilePath, SelectedMemory);
            }, () => SelectedMemory != 0 & FilePath != default);
        }

        public string CurrentRAM => Convert.ToString(_ramCounter.GetFreeRam() / 1024) + "МБ";

        public string TotalRAM => Convert.ToString(_ramCounter.GetTotalRam() / 1024) + "МБ";

        public int[] AcceptableMemories => _ramCounter.GetAcceptableRangeOfRAM();

        public int _selectedMemory;
        public int SelectedMemory
        {
            get => _selectedMemory;
            set
            {
                _selectedMemory = value;
                RaisePropertyChanged(nameof(SelectedMemory));
                BeginExecutionCommand.RaiseCanExecuteChanged();
            }
        }

        private string _filePath;
        public string FilePath 
        { 
            get => _filePath;
            set 
            {
                _filePath = value;
                RaisePropertyChanged(nameof(FilePath));
                BeginExecutionCommand.RaiseCanExecuteChanged();
            }
        }

        public DelegateCommand FilePathCommand {get;}

        public DelegateCommand BeginExecutionCommand {get;}
    }
}
