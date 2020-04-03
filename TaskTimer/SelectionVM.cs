using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TaskTimer
{
    
    class SelectionVM : INotifyPropertyChanged
    {
        public static List<Key> CommonKeysNumbersRow0to9 => new List<Key>() { Key.D0, Key.D1, Key.D2, Key.D3, Key.D4, Key.D5, Key.D6, Key.D7, Key.D8, Key.D9 };
        public static List<Key> CommonKeysNumpad0to9 => new List<Key>() { Key.NumPad0, Key.NumPad1, Key.NumPad2, Key.NumPad3, Key.NumPad4, Key.NumPad5, Key.NumPad6, Key.NumPad7, Key.NumPad8, Key.NumPad9};
        public static List<Key> CommonKeysLettersRowQtoP => new List<Key>() { Key.Q, Key.W, Key.E, Key.R, Key.T, Key.Y, Key.U, Key.I, Key.O, Key.P };
        public static List<Key> CommonKeysLettersRowAtoL=> new List<Key>() { Key.A, Key.S, Key.D, Key.F, Key.G, Key.H, Key.J, Key.J, Key.K, Key.L };
        public static List<Key> CommonKeysLettersRowZtoM=> new List<Key>() { Key.Z, Key.X, Key.C, Key.V, Key.B, Key.N, Key.M};
        public static List<Key> CommonKeysArrowKeys=> new List<Key>() { Key.Up, Key.Down, Key.Left, Key.Right};

        private void NotifyPropertyChanged([CallerMemberName]String PropertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }
        public event PropertyChangedEventHandler PropertyChanged;

        private String _WindowTitle;

        public String WindowTitle
        {
            get => _WindowTitle;
            set
            {
                _WindowTitle = value;
                NotifyPropertyChanged();
            }
        }

        private String _WindowText;

        public String WindowText
        {
            get => _WindowText;
            set
            {
                _WindowText = value;
                NotifyPropertyChanged();
            }
        }

        private List<Key> _AcceptableInput;

        public SelectionVM()
        {
            WindowText = "Text";
            WindowTitle = "Title";
            AcceptableInput = SelectionVM.CommonKeysNumbersRow0to9;
            AcceptableInput.AddRange(SelectionVM.CommonKeysNumpad0to9);
        }

        public List<Key> AcceptableInput
        {
            get => _AcceptableInput;
            set
            {
                _AcceptableInput = value;
                NotifyPropertyChanged();
            }
        }


    }
}
