using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameUi;
using System.IO;

namespace SampleClient.Screens
{
    internal class SplitScreen : Screen
    {
        private Textbox textbox;

        public SplitScreen(BaseScreenComponent manager) : base(manager)
        {
            Background = new BorderBrush(Color.Gray);                       //Hintergrundfarbe festlegen

            Button backButton = Button.TextButton(manager, "Back");             //Neuen TextButton erzeugen
            backButton.HorizontalAlignment = HorizontalAlignment.Left;          //Links
            backButton.VerticalAlignment = VerticalAlignment.Top;               //Oben
            backButton.LeftMouseClick += (s, e) => { manager.NavigateBack(); }; //KlickEvent festlegen
            Controls.Add(backButton);                                           //Button zum Screen hinzufügen



            //ScrollContainer
            ScrollContainer scrollContainer = new ScrollContainer(manager)  //Neuen ScrollContainer erzeugen
            {
                VerticalAlignment = VerticalAlignment.Stretch,              // 100% Höhe
                HorizontalAlignment = HorizontalAlignment.Stretch           //100% Breite
            };
            Controls.Add(scrollContainer);                                  //ScrollContainer zum Root(Screen) hinzufügen



            //Stackpanel - SubControls werden Horizontal oder Vertikal gestackt
            StackPanel panel = new StackPanel(manager);                 //Neues Stackpanel erzeugen
            panel.VerticalAlignment = VerticalAlignment.Stretch;        //100% Höhe
            scrollContainer.Content = panel;                            //Ein Scroll Container kann nur ein Control beherbergen

            //Label
            Label label = new Label(manager) { Text = "Control Showcase" }; //Neues Label erzeugen
            panel.Controls.Add(label);                                      //Label zu Panel hinzufügen

            Button tB = Button.TextButton(manager, "TEST");
            tB.Background = new TextureBrush(LoadTexture2DFromFile("./test_texture_round.png", manager.GraphicsDevice), TextureBrushMode.Stretch);
            panel.Controls.Add(tB);

            //Button
            Button button = Button.TextButton(manager, "Dummy Button"); //Neuen TextButton erzeugen
            panel.Controls.Add(button);                                 //Button zu Panel hinzufügen

            //Progressbar
            ProgressBar pr = new ProgressBar(manager)                   //Neue ProgressBar erzeugen
            {
                Value = 99,                                             //Aktueller Wert
                Height = 20,                                            //Höhe
                Width = 200                                             //Breite
            };      
            panel.Controls.Add(pr);                                     //ProgressBar zu Panel hinzufügen

            //ListBox
            Listbox<string> list = new Listbox<string>(manager);        //Neue ListBox erstellen
            list.TemplateGenerator = (item) =>                          //Template Generator festlegen
            {
                return new Label(manager) { Text = item };              //Control (Label) erstellen
            };
            panel.Controls.Add(list);                                   //Liste zu Panel hinzufügen

            list.Items.Add("Hallo");                                    //Items zur Liste hinzufügen
            list.Items.Add("Welt");                                     //...

            //Combobox
            Combobox<string> combobox = new Combobox<string>(manager)   //Neue Combobox erstellen
            {
                Height = 20,                                            //Höhe 20
                Width = 100                                             //Breite 100
            };
            combobox.TemplateGenerator = (item) =>                      //Template Generator festlegen
            {
                return new Label(manager) { Text = item };              //Control (Label) erstellen
            };
            panel.Controls.Add(combobox);                               //Combobox zu Panel  hinzufügen

            combobox.Items.Add("Combobox");                             //Items zu Combobox hinzufügen
            combobox.Items.Add("Item");
            combobox.Items.Add("Hallo");
           

            Button clearCombobox = Button.TextButton(manager, "Clear Combobox");
            clearCombobox.LeftMouseClick += (s, e) => {
                combobox.Items.Clear();
                list.Items.Clear();
            };
            panel.Controls.Add(clearCombobox);

            //Slider Value Label
            Label labelSliderHorizontal = new Label(manager);

            //Horizontaler Slider
            Slider sliderHorizontal = new Slider(manager)
            {
                Width = 150,
                Height = 20,
            };
            sliderHorizontal.ValueChanged += (value) => { labelSliderHorizontal.Text = "Value: " + value; }; //Event on Value Changed
            panel.Controls.Add(sliderHorizontal);
            labelSliderHorizontal.Text = "Value: " + sliderHorizontal.Value;                                 //Set Text initially
            panel.Controls.Add(labelSliderHorizontal);

            //Slider Value Label
            Label labelSliderVertical = new Label(manager);

            //Vertikaler Slider
            Slider sliderVertical = new Slider(manager)
            {
                Range = 100,
                Height = 200,
                Width = 20,
                Orientation = Orientation.Vertical
            };
            sliderVertical.ValueChanged += (value) => { labelSliderVertical.Text = "Value: " + value; };
            panel.Controls.Add(sliderVertical);
            labelSliderVertical.Text = "Value: " + sliderVertical.Value;
            panel.Controls.Add(labelSliderVertical);

            Checkbox checkbox = new Checkbox(manager);
            panel.Controls.Add(checkbox);


            //Textbox   
            textbox = new Textbox(manager)                      //Neue TextBox erzeugen
            {
                Background = new BorderBrush(Color.LightGray),          //Festlegen eines Backgrounds für ein Control
                HorizontalAlignment = HorizontalAlignment.Stretch,          //100% Breite
                Text = "TEXTBOX!",                                      //Voreingestellter text
                MinWidth = 100                                          //Eine Textbox kann ihre Größe automatisch anpassen
            };

            Button clearTextbox = new Button(manager);
            clearTextbox.LeftMouseClick += (s, e) =>
            {
                textbox.SelectionStart = 0;
                textbox.Text = "";
            };
            panel.Controls.Add(clearTextbox);
            panel.Controls.Add(textbox);                                //Textbox zu Panel hinzufügen

        }

        static Texture2D LoadTexture2DFromFile(string path, GraphicsDevice device)
        {
            using (Stream stream = File.OpenRead(path))
            {
                return Texture2D.FromStream(device, stream);
            }
        }

        protected override void OnKeyDown(KeyEventArgs args)
        {
            if (args.Key == Microsoft.Xna.Framework.Input.Keys.RightAlt)
            {
                textbox.Text = "";
            }
                       base.OnKeyDown(args);
        }
    }

        
}
