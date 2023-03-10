using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Greetings
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GreetingPage : ContentPage
    {
        public GreetingPage()
        {
            StackLayout stackLayout = new StackLayout();
            // Loop through the Color structure fields.
            foreach (FieldInfo info in typeof(Color).GetRuntimeFields())
            {
                // Skip the obsolete (i.e. misspelled) colors.
                if (info.GetCustomAttribute<ObsoleteAttribute>() != null)
                    continue;
                if (info.IsPublic &&
                info.IsStatic &&
                info.FieldType == typeof(Color))
                {
                    stackLayout.Children.Add(
                    CreateColorLabel((Color)info.GetValue(null), info.Name));
                }
            }
            // Loop through the Color structure properties.
            foreach (PropertyInfo info in typeof(Color).GetRuntimeProperties())
            {
                MethodInfo methodInfo = info.GetMethod;
                if (methodInfo.IsPublic &&
                methodInfo.IsStatic &&
                methodInfo.ReturnType == typeof(Color))
                {
                    stackLayout.Children.Add(
                    CreateColorLabel((Color)info.GetValue(null), info.Name));
                }
            }
            Padding = new Thickness(5, Device.OnPlatform(20, 5, 5), 5, 5);
            // Put the StackLayout in a ScrollView.
            Content = new ScrollView
            {
                Content = stackLayout
            };
        }

        Label CreateColorLabel(Color color, string name)
        {
            Color backgroundColor = Color.Default;
            if (color != Color.Default)
            {
                // Standard luminance calculation.
                double luminance = 0.30 * color.R +
                0.59 * color.G +
                0.11 * color.B;
                backgroundColor = luminance > 0.5 ? Color.Black : Color.White;
            }
            // Create the Label.
            return new Label
            {
                Text = name,
                TextColor = color,
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                BackgroundColor = backgroundColor
            };
        }

    }
}