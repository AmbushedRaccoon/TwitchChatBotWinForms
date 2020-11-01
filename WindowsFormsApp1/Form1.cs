using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApp1.Properties;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private static CancellationTokenSource cancellation;
        private static Task chatTask;

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            StopChatTask();
        }

        private void connectButton_Click(object sender, EventArgs e)
        {
            SaveUserSettings();
            StopChatTask();

            TwitchChatBot.TwitchIRCClient client = new TwitchChatBot.TwitchIRCClient(outputTextBox,
                accountNameInput.Text,
                botNameInput.Text,
                authInput.Text);
            client.Connect();
            cancellation = new CancellationTokenSource();
            chatTask = Task.Run(() => client.Chat(cancellation.Token));
        }

        private void SaveUserSettings()
        {
            Settings.Default["AccountName"] = accountNameInput.Text;
            Settings.Default["BotName"] = botNameInput.Text;
            Settings.Default["AuthToken"] = authInput.Text;
            Settings.Default.Save();
        }

        private void LoadUserSettings()
        {
            accountNameInput.Text = Properties.Settings.Default["AccountName"].ToString();
            botNameInput.Text = Properties.Settings.Default["BotName"].ToString();
            authInput.Text = Properties.Settings.Default["AuthToken"].ToString();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadUserSettings();
        }

        private void StopChatTask()
        {
            if (chatTask != null)
            {
                cancellation.Cancel();
            }
        }
    }
}
