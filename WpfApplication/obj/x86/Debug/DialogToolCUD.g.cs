﻿#pragma checksum "..\..\..\DialogToolCUD.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "F6836F6D132C0DFD8CA90180B1B4B66D"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.276
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace WpfApplication {
    
    
    /// <summary>
    /// DialogToolCUD
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
    public partial class DialogToolCUD : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 5 "..\..\..\DialogToolCUD.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid gridToolCUD;
        
        #line default
        #line hidden
        
        
        #line 7 "..\..\..\DialogToolCUD.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RowDefinition RowId;
        
        #line default
        #line hidden
        
        
        #line 18 "..\..\..\DialogToolCUD.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label labelToolId;
        
        #line default
        #line hidden
        
        
        #line 19 "..\..\..\DialogToolCUD.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label labelDescription;
        
        #line default
        #line hidden
        
        
        #line 20 "..\..\..\DialogToolCUD.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox textBoxDescription;
        
        #line default
        #line hidden
        
        
        #line 23 "..\..\..\DialogToolCUD.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.StackPanel stackPanelButtons;
        
        #line default
        #line hidden
        
        
        #line 24 "..\..\..\DialogToolCUD.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button buttonOkUpdate;
        
        #line default
        #line hidden
        
        
        #line 25 "..\..\..\DialogToolCUD.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button buttonCancel;
        
        #line default
        #line hidden
        
        
        #line 27 "..\..\..\DialogToolCUD.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label labelBuyDate;
        
        #line default
        #line hidden
        
        
        #line 28 "..\..\..\DialogToolCUD.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Calendar calendarBuydate;
        
        #line default
        #line hidden
        
        
        #line 29 "..\..\..\DialogToolCUD.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox checkBoxIsInUse;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/WpfApplication;component/dialogtoolcud.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\DialogToolCUD.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.gridToolCUD = ((System.Windows.Controls.Grid)(target));
            return;
            case 2:
            this.RowId = ((System.Windows.Controls.RowDefinition)(target));
            return;
            case 3:
            this.labelToolId = ((System.Windows.Controls.Label)(target));
            return;
            case 4:
            this.labelDescription = ((System.Windows.Controls.Label)(target));
            return;
            case 5:
            this.textBoxDescription = ((System.Windows.Controls.TextBox)(target));
            return;
            case 6:
            this.stackPanelButtons = ((System.Windows.Controls.StackPanel)(target));
            return;
            case 7:
            this.buttonOkUpdate = ((System.Windows.Controls.Button)(target));
            
            #line 24 "..\..\..\DialogToolCUD.xaml"
            this.buttonOkUpdate.Click += new System.Windows.RoutedEventHandler(this.buttonOkUpdate_Click);
            
            #line default
            #line hidden
            return;
            case 8:
            this.buttonCancel = ((System.Windows.Controls.Button)(target));
            return;
            case 9:
            this.labelBuyDate = ((System.Windows.Controls.Label)(target));
            return;
            case 10:
            this.calendarBuydate = ((System.Windows.Controls.Calendar)(target));
            return;
            case 11:
            this.checkBoxIsInUse = ((System.Windows.Controls.CheckBox)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}
