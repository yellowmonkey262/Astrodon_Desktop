﻿using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Astrodon
{
    public static class Prompt
    {
        public static string ShowDialog(string text, string caption)
        {
            Form prompt = new Form
            {
                Width = 500,
                Height = 150,
                Text = caption
            };
            Label textLabel = new Label() { Left = 50, Top = 20, Text = text };
            TextBox textBox = new TextBox() { Left = 50, Top = 50, Width = 400 };
            textBox.PasswordChar = '*';
            Button confirmation = new Button() { Text = "Ok", Left = 350, Width = 100, Top = 70 };
            confirmation.Click += (sender, e) => { prompt.Close(); };
            prompt.Controls.Add(confirmation);
            prompt.Controls.Add(textLabel);
            prompt.Controls.Add(textBox);
            prompt.ShowDialog();
            return textBox.Text;
        }
    }
}