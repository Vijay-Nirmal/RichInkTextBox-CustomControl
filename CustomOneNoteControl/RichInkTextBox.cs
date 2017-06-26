using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

// The Templated Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234235

namespace CustomOneNoteControl
{
    public sealed class RichInkTextBox : Control
    {
        public RichInkTextBox()
        {
            this.DefaultStyleKey = typeof(RichInkTextBox);
        }

        InkCanvas PATH_INK_CANVAS;
        RichEditBox PATH_RICH_EDIT_BOX;

        protected override void OnApplyTemplate()
        {
            PATH_INK_CANVAS = GetTemplateChild<InkCanvas>("PATH_INK_CANVAS");
            PATH_INK_CANVAS.InkPresenter.InputDeviceTypes = CoreInputDeviceTypes.Mouse | CoreInputDeviceTypes.Pen;

            PATH_RICH_EDIT_BOX = GetTemplateChild<RichEditBox>("PATH_RICH_EDIT_BOX");
        }

        T GetTemplateChild<T>(string elementName) where T : DependencyObject
        {
            var element = GetTemplateChild(elementName) as T;
            if (element == null)
                throw new NullReferenceException(elementName);
            return element;
        }

        public void ChangeMode()
        {
            PATH_RICH_EDIT_BOX.IsEnabled = !PATH_RICH_EDIT_BOX.IsEnabled;
            SetValue(ModeProperty, PATH_RICH_EDIT_BOX.IsEnabled ? Modes.TextEditing : Modes.Inking);
        }

        public InkCanvas InkCanvas
        {
            get => (InkCanvas)GetValue(InkCanvasProperty);
            private set => SetValue(InkCanvasProperty, value);
        }

        // Using a DependencyProperty as the backing store for InkCanvas.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty InkCanvasProperty =
            DependencyProperty.Register("InkCanvas", typeof(InkCanvas), typeof(RichInkTextBox), new PropertyMetadata(null));



        public Modes Mode
        {
            get { return (Modes)GetValue(ModeProperty); }
            set
            {
                PATH_RICH_EDIT_BOX.IsEnabled = (value == Modes.TextEditing) ? true : false;
                SetValue(ModeProperty, value);
            }
        }

        // Using a DependencyProperty as the backing store for Mode.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ModeProperty =
            DependencyProperty.Register("Mode", typeof(Modes), typeof(RichInkTextBox), new PropertyMetadata(Modes.TextEditing));

        public CoreInputDeviceTypes InputDeviceTypes
        {
            get { return (CoreInputDeviceTypes)GetValue(InputDeviceTypesProperty); }
            set
            {
                SetValue(InputDeviceTypesProperty, value);
                PATH_INK_CANVAS.InkPresenter.InputDeviceTypes = value;
            }
        }

        // Using a DependencyProperty as the backing store for InputDeviceTypes.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty InputDeviceTypesProperty =
            DependencyProperty.Register("InputDeviceTypes", typeof(CoreInputDeviceTypes), typeof(RichInkTextBox), new PropertyMetadata(CoreInputDeviceTypes.Mouse | CoreInputDeviceTypes.Pen));

    }

    public enum Modes
    {
        Inking,
        TextEditing
    }
}
