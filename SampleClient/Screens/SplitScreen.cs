﻿using Microsoft.Xna.Framework;
using System;
using MonoGameUi;

namespace SampleClient.Screens
{
    internal class SplitScreen : Screen
    {
        public SplitScreen(IScreenManager manager) : base(manager)
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


            //Checkbox
            Checkbox box = new Checkbox(manager);
            box.CheckedChanged += (c) =>
            {
                //Irgendetwas tun wenn checked changed
            };
            panel.Controls.Add(box);


            //Textbox   
            Textbox textbox = new Textbox(manager)                      //Neue TextBox erzeugen
            {
                Background = new BorderBrush(Color.LightGray),          //Festlegen eines Backgrounds für ein Control
                VerticalAlignment = VerticalAlignment.Stretch,          //100% Breite
                Text = "TEXTBOX!",                                      //Voreingestellter text
                MinWidth = 100                                          //Eine Textbox kann ihre Größe automatisch anpassen
            };
            panel.Controls.Add(textbox);                                //Textbox zu Panel hinzufügen



        }
    }
}
