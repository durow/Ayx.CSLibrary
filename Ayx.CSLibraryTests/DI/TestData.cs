using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Ayx.CSLibrary.DI.Tests
{
    public interface IWeapon
    {
        int Damage { get; set; }
    }

    public class Sword : IWeapon
    {
        public int Damage { get; set; }
    }

    public class Knife:IWeapon
    {
        public int Damage { get; set; }
    }

    public class TestWin:UserControl
    { }

    public class TestViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
