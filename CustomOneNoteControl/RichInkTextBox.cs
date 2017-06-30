using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Input.Inking;
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

        protected override void OnApplyTemplate()
        {
            InkCanvas = GetTemplateChild<InkCanvas>("PATH_INK_CANVAS");
            InkCanvas.InkPresenter.InputDeviceTypes = CoreInputDeviceTypes.Mouse | CoreInputDeviceTypes.Pen;
            InkCanvas.InkPresenter.StrokeInput.StrokeEnded += StrokeInput_StrokeEndedAsync;

            RichEditBox = GetTemplateChild<RichEditBox>("PATH_RICH_EDIT_BOX");
        }

        private async void StrokeInput_StrokeEndedAsync(InkStrokeInput sender, PointerEventArgs args)
        {
            await Task.Delay(100);

            var XBound = InkCanvas.InkPresenter.StrokeContainer.BoundingRect.Bottom;
            if (XBound > InkCanvas.ActualHeight - 500)
                InkCanvas.Height = XBound + 500;

            var YBound = InkCanvas.InkPresenter.StrokeContainer.BoundingRect.Right;
            if (YBound > InkCanvas.ActualWidth - 500)
                InkCanvas.Width = YBound + 500;
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
            RichEditBox.IsEnabled = !RichEditBox.IsEnabled;
            SetValue(ModeProperty, RichEditBox.IsEnabled ? Modes.TextEditing : Modes.Inking);
        }

        public RichEditBox RichEditBox
        {
            get { return (RichEditBox)GetValue(RichEditBoxProperty); }
            set { SetValue(RichEditBoxProperty, value); }
        }
        
        public static readonly DependencyProperty RichEditBoxProperty =
            DependencyProperty.Register("RichEditBox", typeof(RichEditBox), typeof(RichInkTextBox), new PropertyMetadata(null));

        public InkCanvas InkCanvas
        {
            get => (InkCanvas)GetValue(InkCanvasProperty);
            private set => SetValue(InkCanvasProperty, value);
        }
        
        public static readonly DependencyProperty InkCanvasProperty =
            DependencyProperty.Register("InkCanvas", typeof(InkCanvas), typeof(RichInkTextBox), new PropertyMetadata(null));

        public Modes Mode
        {
            get { return (Modes)GetValue(ModeProperty); }
            set
            {
                RichEditBox.IsEnabled = (value == Modes.TextEditing) ? true : false;
                SetValue(ModeProperty, value);
            }
        }

        public static readonly DependencyProperty ModeProperty =
            DependencyProperty.Register("Mode", typeof(Modes), typeof(RichInkTextBox), new PropertyMetadata(Modes.TextEditing));

        public CoreInputDeviceTypes InputDeviceTypes
        {
            get { return (CoreInputDeviceTypes)GetValue(InputDeviceTypesProperty); }
            set
            {
                SetValue(InputDeviceTypesProperty, value);
                InkCanvas.InkPresenter.InputDeviceTypes = value;
            }
        }
        
        public static readonly DependencyProperty InputDeviceTypesProperty =
            DependencyProperty.Register("InputDeviceTypes", typeof(CoreInputDeviceTypes), typeof(RichInkTextBox), new PropertyMetadata(CoreInputDeviceTypes.Mouse | CoreInputDeviceTypes.Pen));

    }

    public enum Modes
    {
        Inking,
        TextEditing
    }
}
