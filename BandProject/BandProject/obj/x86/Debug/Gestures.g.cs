﻿#pragma checksum "C:\Users\olive\Documents\Visual Studio 2015\Projects\BandProject\BandProject\Gestures.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "D95B18F692252CBF948AE0EE081FB4E3"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BandProject
{
    partial class Gestures : 
        global::Windows.UI.Xaml.Controls.Page, 
        global::Windows.UI.Xaml.Markup.IComponentConnector,
        global::Windows.UI.Xaml.Markup.IComponentConnector2
    {
        /// <summary>
        /// Connect()
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 14.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 1:
                {
                    global::Windows.UI.Xaml.Controls.Button element1 = (global::Windows.UI.Xaml.Controls.Button)(target);
                    #line 23 "..\..\..\Gestures.xaml"
                    ((global::Windows.UI.Xaml.Controls.Button)element1).Click += this.remove_Click;
                    #line default
                }
                break;
            case 2:
                {
                    this.LayoutRoot = (global::Windows.UI.Xaml.Controls.Grid)(target);
                }
                break;
            case 3:
                {
                    this.header = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 4:
                {
                    this.GestureName = (global::Windows.UI.Xaml.Controls.TextBox)(target);
                    #line 61 "..\..\..\Gestures.xaml"
                    ((global::Windows.UI.Xaml.Controls.TextBox)this.GestureName).TextChanging += this.GestureName_TextChanging;
                    #line default
                }
                break;
            case 5:
                {
                    this.ListName = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 6:
                {
                    this.listview = (global::Windows.UI.Xaml.Controls.ListView)(target);
                    #line 74 "..\..\..\Gestures.xaml"
                    ((global::Windows.UI.Xaml.Controls.ListView)this.listview).SelectionChanged += this.listview_SelectionChanged;
                    #line default
                }
                break;
            case 7:
                {
                    this.AddGesture = (global::Windows.UI.Xaml.Controls.Button)(target);
                    #line 67 "..\..\..\Gestures.xaml"
                    ((global::Windows.UI.Xaml.Controls.Button)this.AddGesture).Click += this.AddGesture_Click;
                    #line default
                }
                break;
            case 8:
                {
                    this.AddStatus = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 9:
                {
                    this.AppName = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            default:
                break;
            }
            this._contentLoaded = true;
        }

        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 14.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::Windows.UI.Xaml.Markup.IComponentConnector GetBindingConnector(int connectionId, object target)
        {
            global::Windows.UI.Xaml.Markup.IComponentConnector returnValue = null;
            return returnValue;
        }
    }
}

