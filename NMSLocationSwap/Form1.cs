using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
//using QuickType;
//using NMSJSON;
using Nmsjson2;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NMSLocationSwap
{
    public partial class Form1 : Form
    {
        private Dictionary<int, string> sn1a;
        private Dictionary<int, string> sn2a;
        private Dictionary<int, string> sn3a;
        private Dictionary<int, string> sn4a;
        private Dictionary<int, string> sn5a;

        private Dictionary<int, string> sn1b;
        private Dictionary<int, string> sn2b;
        private Dictionary<int, string> sn3b;
        private Dictionary<int, string> sn4b;
        private Dictionary<int, string> sn5b;

        public IDictionary<string, string> galaxyDict;
        private List<String> _changedFiles = new List<string>();

        public Form1()
        {
            InitializeComponent();

            //nmsPath = @"C:\Users\Kevin\AppData\Roaming\HelloGames\NMS\st_76561198267671627";
            nmsPath = Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "HelloGames"), "NMS");
            savePath = System.Windows.Forms.Application.CommonAppDataPath + "\\save.nmsls";

            //toggle debug 
            listView1.Visible = false;
            button9.Visible = false;

            GIndex();
        }

        public string hgFileDir { get; private set; }
        public string nmsPath { get; private set; }
        public string savePath { get; private set; }
        public int saveslota { get; private set; }
        public int saveslotb { get; private set; }
        public string hgFilePathA { get; private set; }
        public string jsonA { get; private set; }
        public int gamemodeinta { get; private set; }
        public int gamemodeintb { get; private set; }
        public string jsonB { get; private set; }
        public string hgFilePathB { get; private set; }
        public string LocName { get; private set; }
        public string LocToMove { get; private set; }
        public string hxx { get; private set; }
        public string GalacticCoord2 { get; private set; }
        public long hxe { get; private set; }
        public string PortalCode { get; private set; }
        public string GalacticCoord { get; private set; }
        public int Planet { get; set; }
        public int record { get; private set; }
        public string ReadKey { get; private set; }

        private void Form1_Load(object sender, EventArgs e)
        {
            //Save preference file
            BuildSaveFile();
            ReloadSave();

            //Make sure backup dir exists or create it
            CreateBackupDir();
        }
        private async void Form1_Shown(object sender, EventArgs e)
        {
            SetPath();

            LoadCmbxA();
            LoadCmbxB();

            fileSystemWatcher1.Path = hgFileDir;

            //give time for form to show
            await Task.Delay(300);
            //RunBackupAll(hgFileDir);
        }
        private void SetPath()
        {
            //Main Save.hg file and directory Finder
            if (Directory.Exists(nmsPath))
            {
                DirectoryInfo dinfo = new DirectoryInfo(nmsPath);
                if (dinfo.GetFiles("save*.hg", SearchOption.AllDirectories).Length > 0)
                {
                    //If only one dir is found, do the following checks
                    if (dinfo.GetFiles("save*.hg", SearchOption.TopDirectoryOnly).Length > 0)
                    {
                        //Check if hg files are here if so set nmsPath
                        nmsPath = dinfo.FullName;
                        Write("nmsPath", nmsPath, savePath);
                        AppendLine(textBox7, "Set Dir: " + nmsPath);
                        return;
                    }

                    if (dinfo.GetDirectories("st_*", SearchOption.TopDirectoryOnly).Length > 0)
                    {
                        //Check for Steam Folder, if found set nmsPath
                        DirectoryInfo[] dirname1 = dinfo.GetDirectories("st_*", SearchOption.TopDirectoryOnly);
                        nmsPath = dirname1[0].FullName;
                        Write("nmsPath", nmsPath, savePath);
                        AppendLine(textBox7, "Set Dir: " + nmsPath);
                        return;
                    }

                    if (dinfo.GetDirectories("DefaultUser", SearchOption.TopDirectoryOnly).Length > 0)
                    {
                        //Check for GoG Folder, if found set nmsPath
                        DirectoryInfo[] dirname2 = dinfo.GetDirectories("DefaultUser", SearchOption.TopDirectoryOnly);
                        nmsPath = dirname2[0].FullName;
                        Write("nmsPath", nmsPath, savePath);
                        AppendLine(textBox7, "Set Dir: " + nmsPath);
                        return;
                    }
                }
            }
        }        
        public static void AppendLine(TextBox source, string value)
        {
            //My neat little textbox handler
            if (source.Text.Length == 0)
                source.Text = value;
            else
                source.AppendText("\r\n" + value);
        }
        public void LoadCmbxA()
        {
            //Load save file names in combobox1 

            //If nmsPath is not found, show message and return
            if (!Directory.Exists(nmsPath))
            {
                MessageBox.Show("No Man's Sky save game folder not found, select it manually!", "Alert", MessageBoxButtons.OK);
                return;
            }

            //Search for hg files in the current dir
            DirectoryInfo dinfo = new DirectoryInfo(nmsPath);
            FileInfo[] Files = dinfo.GetFiles("save*.hg", SearchOption.TopDirectoryOnly);// SearchOption.AllDirectories);

            //if hg files are found, start adding them to dictionaries
            if (Files.Length != 0)
            {
                Dictionary<int, string> sl1 = new Dictionary<int, string>();
                sn1a = new Dictionary<int, string>();
                sn2a = new Dictionary<int, string>();
                sn3a = new Dictionary<int, string>();
                sn4a = new Dictionary<int, string>();
                sn5a = new Dictionary<int, string>();

                foreach (FileInfo file in Files.OrderByDescending(f => f.LastWriteTime))
                {
                    if (file.Name == "save.hg" | file.Name == "save2.hg")
                    {
                        if (!sn1a.ContainsKey(1))
                            sn1a.Add(1, file.Name);
                        else sn1a.Add(2, file.Name);

                        if (!sl1.ContainsValue("Slot 1"))
                            sl1.Add(1, "Slot 1");
                    }
                    if (file.Name == "save3.hg" | file.Name == "save4.hg")
                    {
                        if (!sn2a.ContainsKey(3))
                            sn2a.Add(3, file.Name);
                        else sn2a.Add(4, file.Name);

                        if (!sl1.ContainsValue("Slot 2"))
                            sl1.Add(2, "Slot 2");
                    }
                    if (file.Name == "save5.hg" | file.Name == "save6.hg")
                    {
                        if (!sn3a.ContainsKey(5))
                            sn3a.Add(5, file.Name);
                        else sn3a.Add(6, file.Name);

                        if (!sl1.ContainsValue("Slot 3"))
                            sl1.Add(3, "Slot 3");
                    }
                    if (file.Name == "save7.hg" | file.Name == "save8.hg")
                    {
                        if (!sn4a.ContainsKey(7))
                            sn4a.Add(7, file.Name);
                        else sn4a.Add(8, file.Name);

                        if (!sl1.ContainsValue("Slot 4"))
                            sl1.Add(4, "Slot 4");
                    }
                    if (file.Name == "save9.hg" | file.Name == "save10.hg")
                    {
                        if (!sn5a.ContainsKey(9))
                            sn5a.Add(9, file.Name);
                        else sn5a.Add(10, file.Name);

                        if (!sl1.ContainsValue("Slot 5"))
                            sl1.Add(5, "Slot 5");
                    }
                }

                sl1.Add(0, "(Select Save Slot)");
                comboBox2.DataSource = sl1.ToArray();
                comboBox2.DisplayMember = "VALUE";
                comboBox2.ValueMember = "KEY";

                hgFileDir = Path.GetDirectoryName(Files[0].FullName);
                //fileSystemWatcher1.Path = hgFileDir;

                textBox8.Clear();
                AppendLine(textBox8, hgFileDir);
            }
            else
            {
                AppendLine(textBox7, "No save files found!");
                return;
            }
        }
        public void LoadCmbxB()
        {
            //Load save file names in combobox1 

            //If nmsPath is not found, show message and return
            if (!Directory.Exists(nmsPath))
            {
                MessageBox.Show("No Man's Sky save game folder not found, select it manually!", "Alert", MessageBoxButtons.OK);
                return;
            }

            //Search for hg files in the current dir
            DirectoryInfo dinfo = new DirectoryInfo(nmsPath);
            FileInfo[] Files = dinfo.GetFiles("save*.hg", SearchOption.TopDirectoryOnly);// SearchOption.AllDirectories);

            //if hg files are found, start adding them to dictionaries
            if (Files.Length != 0)
            {
                Dictionary<int, string> sl1 = new Dictionary<int, string>();
                sn1b = new Dictionary<int, string>();
                sn2b = new Dictionary<int, string>();
                sn3b = new Dictionary<int, string>();
                sn4b = new Dictionary<int, string>();
                sn5b = new Dictionary<int, string>();

                foreach (FileInfo file in Files.OrderByDescending(f => f.LastWriteTime))
                {
                    if (file.Name == "save.hg" | file.Name == "save2.hg")
                    {
                        if (!sn1b.ContainsKey(1))
                            sn1b.Add(1, file.Name);
                        else sn1b.Add(2, file.Name);

                        if (!sl1.ContainsValue("Slot 1"))
                            sl1.Add(1, "Slot 1");
                    }
                    if (file.Name == "save3.hg" | file.Name == "save4.hg")
                    {
                        if (!sn2b.ContainsKey(3))
                            sn2b.Add(3, file.Name);
                        else sn2b.Add(4, file.Name);

                        if (!sl1.ContainsValue("Slot 2"))
                            sl1.Add(2, "Slot 2");
                    }
                    if (file.Name == "save5.hg" | file.Name == "save6.hg")
                    {
                        if (!sn3b.ContainsKey(5))
                            sn3b.Add(5, file.Name);
                        else sn3b.Add(6, file.Name);

                        if (!sl1.ContainsValue("Slot 3"))
                            sl1.Add(3, "Slot 3");
                    }
                    if (file.Name == "save7.hg" | file.Name == "save8.hg")
                    {
                        if (!sn4b.ContainsKey(7))
                            sn4b.Add(7, file.Name);
                        else sn4b.Add(8, file.Name);

                        if (!sl1.ContainsValue("Slot 4"))
                            sl1.Add(4, "Slot 4");
                    }
                    if (file.Name == "save9.hg" | file.Name == "save10.hg")
                    {
                        if (!sn5b.ContainsKey(9))
                            sn5b.Add(9, file.Name);
                        else sn5b.Add(10, file.Name);

                        if (!sl1.ContainsValue("Slot 5"))
                            sl1.Add(5, "Slot 5");
                    }
                }

                sl1.Add(0, "(Select Save Slot)");
                comboBox4.DataSource = sl1.ToArray();
                comboBox4.DisplayMember = "VALUE";
                comboBox4.ValueMember = "KEY";

                hgFileDir = Path.GetDirectoryName(Files[0].FullName);
                //fileSystemWatcher1.Path = hgFileDir;

                textBox9.Clear();
                AppendLine(textBox9, hgFileDir);
            }
            else
            {
                AppendLine(textBox7, "No save files found!");
                return;
            }
        }
        private void ComboBox2_SelectionChangeCommitted(object sender, EventArgs e)
        {
            string selected = this.comboBox2.GetItemText(this.comboBox2.SelectedItem);

            //Gets the dictionaries set in loadcmbbx and sets the data source for save dropdown
            if (selected == "Slot 1")
            {
                saveslota = 1;
                comboBox1.DisplayMember = "VALUE";
                comboBox1.ValueMember = "KEY";
                comboBox1.DataSource = sn1a.ToArray();
                return;
            }
            if (selected == "Slot 2")
            {
                saveslota = 2;
                comboBox1.DisplayMember = "VALUE";
                comboBox1.ValueMember = "KEY";
                comboBox1.DataSource = sn2a.ToArray();
                return;
            }
            if (selected == "Slot 3")
            {
                saveslota = 3;
                comboBox1.DisplayMember = "VALUE";
                comboBox1.ValueMember = "KEY";
                comboBox1.DataSource = sn3a.ToArray();
                return;
            }
            if (selected == "Slot 4")
            {
                saveslota = 4;
                comboBox1.DisplayMember = "VALUE";
                comboBox1.ValueMember = "KEY";
                comboBox1.DataSource = sn4a.ToArray();
                return;
            }
            if (selected == "Slot 5")
            {
                saveslota = 5;
                comboBox1.DisplayMember = "VALUE";
                comboBox1.ValueMember = "KEY";
                comboBox1.DataSource = sn5a.ToArray();
                return;
            }
            if (selected == "(Select Save Slot)")
            {
                saveslota = -1;
                comboBox1.DataSource = null;
                ClearA();
                LoadCmbxA();
                return;
            }
        }
        private void ComboBox4_SelectionChangeCommitted(object sender, EventArgs e)
        {
            string selected = this.comboBox4.GetItemText(this.comboBox4.SelectedItem);

            //Gets the dictionaries set in loadcmbbx and sets the data source for save dropdown
            if (selected == "Slot 1")
            {
                saveslotb = 1;
                comboBox3.DisplayMember = "VALUE";
                comboBox3.ValueMember = "KEY";
                comboBox3.DataSource = sn1b.ToArray();
                return;
            }
            if (selected == "Slot 2")
            {
                saveslotb = 2;
                comboBox3.DisplayMember = "VALUE";
                comboBox3.ValueMember = "KEY";
                comboBox3.DataSource = sn2b.ToArray();
                return;
            }
            if (selected == "Slot 3")
            {
                saveslotb = 3;
                comboBox3.DisplayMember = "VALUE";
                comboBox3.ValueMember = "KEY";
                comboBox3.DataSource = sn3b.ToArray();
                return;
            }
            if (selected == "Slot 4")
            {
                saveslotb = 4;
                comboBox3.DisplayMember = "VALUE";
                comboBox3.ValueMember = "KEY";
                comboBox3.DataSource = sn4b.ToArray();
                return;
            }
            if (selected == "Slot 5")
            {
                saveslotb = 5;
                comboBox3.DisplayMember = "VALUE";
                comboBox3.ValueMember = "KEY";
                comboBox3.DataSource = sn5b.ToArray();
                return;
            }
            if (selected == "(Select Save Slot)")
            {
                saveslotb = -1;
                comboBox3.DataSource = null;
                ClearB();
                LoadCmbxB();
                return;
            }
        }

        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //After selecting a saveslot, this triggers + after selecting a different save   
            string selected = this.comboBox1.GetItemText(this.comboBox1.SelectedItem);
            if (selected != "")
            {
                //ClearAll();
                ClearA();
                GetSaveFileA(selected);
                Loadlsb1A();
                //LoadBaselsbx();
                //GetPlayerCoord();
            }
        }
        private void ComboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            //After selecting a saveslot, this triggers + after selecting a different save   
            string selected = this.comboBox3.GetItemText(this.comboBox3.SelectedItem);
            if (selected != "")
            {
                //ClearAll();
                ClearB();
                GetSaveFileB(selected);
                Loadlsb1B();
                //LoadBaselsbx();
                //GetPlayerCoord();
            }
        }
        private void GetSaveFileA(string selected)
        {
            //Main save file loader
            if (Directory.Exists(hgFileDir) && selected != "")
            {
                DirectoryInfo dinfo = new DirectoryInfo(hgFileDir);
                FileInfo[] Files = dinfo.GetFiles(selected, SearchOption.TopDirectoryOnly); //AllDirectories);

                if (Files.Length != 0)
                {
                    foreach (FileInfo file in Files)
                    {
                        //sets the file path to work with
                        hgFilePathA = file.FullName;
                    }
                }
                else
                {
                    AppendLine(textBox7, "** Code 3 ** " + selected);
                    return;
                }

                //shows the file path in the path textbox
                textBox8.Clear();
                AppendLine(textBox8, hgFilePathA);

                //displays the last write time
                FileInfo hgfile = new FileInfo(hgFilePathA);
                textBox1.Clear();
                AppendLine(textBox1, hgfile.LastWriteTime.ToShortDateString() + " " + hgfile.LastWriteTime.ToLongTimeString());

                //Sets json from the selected save file
                jsonA = File.ReadAllText(hgFilePathA);

                //looksup and then displays the game mode
                var nms = Nms.FromJson(jsonA);
                try
                {
                    gamemodeinta = Convert.ToInt32(nms.F2P);
                    GameModeLookupInt(gamemodeinta, label1);
                }
                catch
                {
                    return;
                }
            }
        }
        private void GetSaveFileB(string selected)
        {
            //Main save file loader
            if (Directory.Exists(hgFileDir) && selected != "")
            {
                DirectoryInfo dinfo = new DirectoryInfo(hgFileDir);
                FileInfo[] Files = dinfo.GetFiles(selected, SearchOption.TopDirectoryOnly); //AllDirectories);

                if (Files.Length != 0)
                {
                    foreach (FileInfo file in Files)
                    {
                        //sets the file path to work with
                        hgFilePathB = file.FullName;
                    }
                }
                else
                {
                    AppendLine(textBox7, "** Code 3 ** " + selected);
                    return;
                }

                //shows the file path in the path textbox
                textBox9.Clear();
                AppendLine(textBox9, hgFilePathB);

                //displays the last write time
                FileInfo hgfile = new FileInfo(hgFilePathB);
                textBox2.Clear();
                AppendLine(textBox2, hgfile.LastWriteTime.ToShortDateString() + " " + hgfile.LastWriteTime.ToLongTimeString());

                //Sets json from the selected save file
                jsonB = File.ReadAllText(hgFilePathB);

                //looksup and then displays the game mode
                var nms = Nms.FromJson(jsonB);
                try
                {
                    gamemodeintb = Convert.ToInt32(nms.F2P);
                    GameModeLookupInt(gamemodeintb, label2);
                }
                catch
                {
                    return;
                }
            }
        }
        private void GameModeLookupInt(int mode, Label label)
        {
            //Looks up game mode in ranges to prevent "not found"
            try
            {
                if (mode > 4600 & mode < 4700)
                {
                    label.Text = "Normal";
                }
                if (mode > 5600 & mode < 5700)
                {
                    label.Text = "Survival";
                }
                if (mode > 6600 & mode < 6700)
                {
                    label.Text = "Permadeath";
                }
                if (mode > 5100 & mode < 5200)
                {
                    label.Text = "Creative";
                }
            }
            catch
            {
                return;
            }
        }
        private void Loadlsb1A()
        {
            //Method to load all location discovered in listbox1
            //DiscList.Clear();
            listBox1.Items.Clear();
            //listBox2.Items.Clear();
            //TextBoxes();

            var nms = Nms.FromJson(jsonA);
            try
            {
                for (int i = 0; i < nms.The6F.NlG.Length; i++)
                {
                    string discd = nms.The6F.NlG[i].NKm;
                    ///listBox1.Items.Add(discd);
                    
                    if (nms.The6F.NlG[i].IAf == "Spacestation")
                    {
                        listBox1.Items.Add(discd);
                    }
                    
                    /*
                    if (nms.The6F.NlG[i].IAf == "Spacestation")
                    {
                        string ss = discd + " (SS)";
                        //DiscList.Add(ss);
                        listBox2.Items.Add(ss);
                    }
                    else if (nms.The6F.NlG[i].IAf != "Spacestation")
                    {
                        string bl = discd + " (B)";
                        //DiscList.Add(bl);
                        listBox1.Items.Add(bl);
                    }
                    */
                }
            }
            catch
            {
                AppendLine(textBox7, "** Code 111 **");
                return;
            }
            
            textBox5.Text = listBox1.Items.Count.ToString();
            //textBox20.Text = listBox2.Items.Count.ToString();
            listBox1.SelectedIndex = -1;
        }
        private void Loadlsb1B()
        {
            //Method to load all location discovered in listbox2
            //DiscList.Clear();
            //listBox1.Items.Clear();
            listBox2.Items.Clear();
            //TextBoxes();

            var nms = Nms.FromJson(jsonB);
            try
            {
                for (int i = 0; i < nms.The6F.NlG.Length; i++)
                {
                    string discd = nms.The6F.NlG[i].NKm;
                    ///listBox2.Items.Add(discd);
                    
                    if (nms.The6F.NlG[i].IAf == "Spacestation")
                    {
                        listBox2.Items.Add(discd);
                    }
                    
                    /*
                    if (nms.The6F.NlG[i].IAf == "Spacestation")
                    {
                        string ss = discd + " (SS)";
                        //DiscList.Add(ss);
                        listBox2.Items.Add(ss);
                    }
                    else if (nms.The6F.NlG[i].IAf != "Spacestation")
                    {
                        string bl = discd + " (B)";
                        //DiscList.Add(bl);
                        listBox1.Items.Add(bl);
                    }
                    */
                }
            }
            catch
            {
                AppendLine(textBox7, "** Code 111 **");
                return;
            }

            textBox6.Text = listBox2.Items.Count.ToString();
            //textBox20.Text = listBox2.Items.Count.ToString();
            listBox2.SelectedIndex = -1;
        }
        private void ShowCoord(string basename, string json)
        {
            textBox10.Clear();
            textBox11.Clear();
            textBox12.Clear();
            textBox13.Clear();

            var nms = Nms.FromJson(json);

            for (int i = 0; i < nms.The6F.NlG.Length; i++)
            {
                if (nms.The6F.NlG[i].NKm == basename)
                {
                    //lst.Add(discov);
                    Planet = Convert.ToInt32(nms.The6F.NlG[i].YhJ.OZw["jsv"]);
                    var X = Convert.ToInt32(nms.The6F.NlG[i].YhJ.OZw["dZj"]);
                    var Y = Convert.ToInt32(nms.The6F.NlG[i].YhJ.OZw["IyE"]);
                    var Z = Convert.ToInt32(nms.The6F.NlG[i].YhJ.OZw["uXE"]);
                    var SSI = Convert.ToInt32(nms.The6F.NlG[i].YhJ.OZw["vby"]);
                    VoxelToGalacticCoord(X, Y, Z, SSI, false);

                    //item = new ListViewItem(nms.The6F.NlG[i].IAf, 1);

                    string[] value = GalacticCoord.Replace(" ", "").Split(':');
                    string A = value[0].Trim();
                    string B = value[1].Trim();
                    string C = value[2].Trim();
                    string D = value[3].Trim();

                    //Validate Coordinates
                    if (ValidateCoord(A, B, C, D))
                    {
                        MessageBox.Show("Invalid Coordinates! Out of Range!", "Alert");
                        //Clear();
                        AppendLine(textBox7, "Invalid Coordinates!");
                        continue;
                    }
                    GalacticToPortal(Planet, A, B, C, D, false);

                    textBox10.Text = basename;
                    textBox11.Text = GalacticCoord;
                    textBox12.Text = PortalCode;
                    GalaxyLookup(textBox13, nms.The6F.NlG[i].YhJ.Iis.ToString());


                    /*
                    subItems = new ListViewItem.ListViewSubItem[]
                    { new ListViewItem.ListViewSubItem(item, PortalCode),
                        new ListViewItem.ListViewSubItem(item, GalacticCoord),
                        new ListViewItem.ListViewSubItem(item, nms.The6F.NlG[i].NKm)};

                    item.SubItems.AddRange(subItems);
                    listView1.Items.Add(item);
                    */
                }
            }
        }
        private void ListBox1_MouseClick(object sender, EventArgs e)
        {            
            //When a location is clicked on listbox1
            listBox2.SelectedIndex = -1;

            string selected = listBox1.GetItemText(listBox1.SelectedItem);
            if (selected == "")
            {
                MessageBox.Show("Something Went Wrong", "Error", MessageBoxButtons.OK);
                return;
            }
            
            ShowCoord(selected, jsonA);

            LocToMove = listBox1.GetItemText(listBox1.SelectedItem);            
        }
        private void ListBox2_MouseClick(object sender, EventArgs e)
        {            
            //When a location is clicked on listbox2
            listBox1.SelectedIndex = -1;

            string selected = listBox2.GetItemText(listBox2.SelectedItem);
            if (selected == "")
            {
                MessageBox.Show("Something Went Wrong", "Error", MessageBoxButtons.OK);
                return;
            }

            ShowCoord(selected, jsonB);

            LocToMove = listBox2.GetItemText(listBox2.SelectedItem);
        }
        private void Button5_Click(object sender, EventArgs e)
        {
            //Search A
            //Clearforsearch();
            if (listBox1.Items.Count >= 1)
                listBox1.SelectedIndex = Find(listBox1, textBox3.Text, listBox1.SelectedIndex + 1);
            if (listBox1.SelectedIndex != -1)
                ListBox1_MouseClick(this, new EventArgs());
        }
        private void Button6_Click(object sender, EventArgs e)
        {
            //Search B
            //Clearforsearch();
            if (listBox2.Items.Count >= 1)
                listBox2.SelectedIndex = Find(listBox2, textBox4.Text, listBox2.SelectedIndex + 1);
            if (listBox2.SelectedIndex != -1)
                ListBox2_MouseClick(this, new EventArgs());
        }
        int Find(ListBox lb, string searchString, int startIndex)
        {
            //Find method for search bars on top of listboxes
            for (int i = startIndex; i < lb.Items.Count; ++i)
            {
                //string lbString = lb.Items[i].ToString();
                if (lb.Items[i].ToString().IndexOf(searchString, StringComparison.OrdinalIgnoreCase) >= 0)
                    return i;
            }
            return -1; //Find(lb, searchString, 0);
        }

        private GameSave _gs;
        private GameSaveManager _gsm;
        private uint _gameSlot;

        private void WriteSave(ProgressBar pb, TextBox tb, int saveslot, string path, string loc, List<string> list)
        {
            //Main method for writing a change
            //BackUpSaveSlot(tb, saveslot, false, path);
            //DecryptSave(saveslot, path);
            //EditSaveFB(pb);

            //EditSaveLoc(loc, list);

            //EncryptSave(pb, saveslot);
        }
        private void RunBackupAll(string Path)
        {
            DoCommon();

            try
            {
                string archivePath = Path; //hgFileDir;

                if (Directory.Exists(Path))
                {
                    //var baseName = string.Format("nmssavetool-backupall-{0}", _gsm.FindMostRecentSaveDateTime().ToString("yyyyMMdd-HHmmss"));
                    var basePath = @".\backup\nms-backup-" + DateTime.Now.ToString("yyyy-MM-dd-HHmmss");
                    archivePath = basePath + ".zip";
                    _gsm.ArchiveSaveDirTo(archivePath);
                    AppendLine(textBox7, "All saves backed up to zip file created in \\backup folder...");
                }
            }
            catch
            {
                MessageBox.Show("No Man's Sky save game folder not found, select it manually!", "Alert", MessageBoxButtons.OK);
            }
        }
        public void BackUpSaveSlot(TextBox tb, int slot, bool msg, string path)
        {
            //Backup a single save slot Method
            if (slot >= 1 && slot <= 5)
            {
                string hgFileName = Path.GetFileNameWithoutExtension(path);

                string mf_hgFilePath = path;
                mf_hgFilePath = String.Format("{0}{1}{2}{3}", Path.GetDirectoryName(mf_hgFilePath) + @"\", "mf_", Path.GetFileNameWithoutExtension(mf_hgFilePath), Path.GetExtension(mf_hgFilePath));

                string mf_hgFileName = Path.GetFileNameWithoutExtension(mf_hgFilePath);

                if (Directory.Exists(@".\temp"))
                {
                    Directory.Delete(@".\temp", true);
                }

                Directory.CreateDirectory(@".\temp");

                File.Copy(path, @".\temp\" + hgFileName + Path.GetExtension(path));
                File.Copy(mf_hgFilePath, @".\temp\" + mf_hgFileName + Path.GetExtension(mf_hgFilePath));

                string datetime = DateTime.Now.ToString("yyyy-MM-dd-HHmmss");
                ZipFile.CreateFromDirectory(@".\temp", @".\backup\savebackup_" + slot + "_" + datetime + ".zip");

                Directory.Delete(@".\temp", true);

                if (File.Exists(@".\backup\savebackup_" + slot + "_" + datetime + ".zip")) //@".\backup\" + GetNewestZip(@".\backup")))
                {
                    AppendLine(tb, "Save file on Slot: ( " + slot + " ) backed up to \\backup folder...");

                    if (msg == true)
                    {
                        MessageBox.Show("Save slot backup up to: savebackup_" + slot + "_" + datetime + ".zip", "Save Backup", MessageBoxButtons.OK);
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    if (msg == true)
                    {
                        MessageBox.Show("No File backed up!", "Alert");
                    }
                    else
                    {
                        AppendLine(tb, "Something went wrong, no file backed up Error BU:5567");
                        return;
                    }
                }
            }
            else
            {
                if (msg == true)
                {
                    MessageBox.Show("No File Found / Select Save Slot!", "Alert");
                }
                else
                {
                    return;
                }
            }
        }
        private void DecryptSave(int saveslot, string path, string AorB)
        {
            LoadRun(saveslot, path);
            if (AorB == "A")
            {
                RunDecryptA();
            }
            if (AorB == "B")
            {
                RunDecryptB();
            }            
        }
        private void EncryptSave(ProgressBar pb, int saveslot)
        {
            RunEncrypt(pb, saveslot);
        }
        private void LoadRun(int saveslot, string path)
        {
            uint GameSlot = Convert.ToUInt32(saveslot);

            DoGameSlotCommon(saveslot);

            try
            {
                string mf_hgFilePath = path;
                mf_hgFilePath = String.Format("{0}{1}{2}{3}", Path.GetDirectoryName(mf_hgFilePath) + @"\", "mf_", Path.GetFileNameWithoutExtension(mf_hgFilePath), Path.GetExtension(mf_hgFilePath));

                //Sets the save to be the last modified
                File.SetLastWriteTime(mf_hgFilePath, DateTime.Now);
                File.SetLastWriteTime(path, DateTime.Now);

                _gs = _gsm.ReadSaveFile(GameSlot);
            }
            catch
            {
                return;
            }
        }
        private void RunDecryptA()
        {
            //Parsing and formatting save game JSON
            string formattedJson;

            try
            {
                formattedJson = _gs.ToFormattedJsonString();
                File.WriteAllText(@".\backup\json\locsaveA.json", formattedJson);
            }
            catch
            {
                return;
            }
        }
        private void RunDecryptB()
        {
            //Parsing and formatting save game JSON
            string formattedJson;

            try
            {
                formattedJson = _gs.ToFormattedJsonString();
                File.WriteAllText(@".\backup\json\locsaveB.json", formattedJson);
            }
            catch
            {
                return;
            }
        }
        private void RunEncrypt(ProgressBar pb, int saveslot)
        {
            DoGameSlotCommon(saveslot);

            try
            {
                //Read edited saveedit.json
                _gs = _gsm.ReadUnencryptedGameSave(@".\backup\json\locsaveedit.json");
            }
            catch
            {
                return;
            }
            try
            {
                //Write and Encrypt new save files
                _gsm.WriteSaveFile(_gs, _gameSlot);
                pb.Invoke((Action)(() => pb.Value = 90));
            }
            catch
            {
                return;
            }
        }
        private void DoGameSlotCommon(int saveslot)
        {
            DoCommon();
            _gameSlot = Convert.ToUInt32(saveslot);
        }
        private void DoCommon()
        {
            _gsm = new GameSaveManager(hgFileDir);
        }
        private void SetWriteTime(string path)
        {
            string mf_hgFilePath = path;
            mf_hgFilePath = String.Format("{0}{1}{2}{3}", Path.GetDirectoryName(mf_hgFilePath) + @"\", "mf_", Path.GetFileNameWithoutExtension(mf_hgFilePath), Path.GetExtension(mf_hgFilePath));

            //Sets the save to be the last modified
            File.SetLastWriteTime(mf_hgFilePath, DateTime.Now);
            File.SetLastWriteTime(path, DateTime.Now);
        }
        private void DelLoc(string name, string path) //string json)
        {
            //INPUT is jsonA.json or jsonB.json

            record = -1;

            string json = File.ReadAllText(path);
            var j = JObject.Parse(json);
            var readfile = JsonConvert.SerializeObject(j, Formatting.None);
            //File.WriteAllText(@".\backup\json\nlg_src.json", readfile);

            //Set nms from readfile
            var nms = Nms.FromJson(readfile);

            //Get NlG to print to file indented and for replacement later
            var nlgold = JsonConvert.SerializeObject(nms.The6F.NlG, Formatting.None);
            //var nlgo = JsonConvert.SerializeObject(nms.The6F.NlG, Formatting.Indented);
            //File.WriteAllText(@".\backup\json\nlg_orig.json", nlgo);

            for (int i = 0; i < nms.The6F.NlG.Length; i++)
            {
                if (nms.The6F.NlG[i].NKm == name)
                {
                    record = i;
                }
            }            
            if (record == -1)
            {
                MessageBox.Show("Record not found!", "Alert", MessageBoxButtons.OK);
                return;
            }

            //Remove record from NlG
            nms.The6F.NlG = nms.The6F.NlG.Where(val => val != nms.The6F.NlG[record]).ToArray();

            //Get NlG tp print to file and for replacement later
            var nlgn = JsonConvert.SerializeObject(nms.The6F.NlG, Formatting.Indented);
            var nlgnew = JsonConvert.SerializeObject(nms.The6F.NlG, Formatting.None);
            //File.WriteAllText(@".\backup\json\nlg_dest.json", nlgn);

            //Replace the old NlG with the new NlG
            readfile = readfile.Replace(nlgold, nlgnew);
            //json = json.Replace(nlgold, nlgnew);
            //File.WriteAllText(@".\backup\json\nlg_edit.json", JsonConvert.SerializeObject(JObject.Parse(json), Formatting.Indented));

            //Write to file for encrypt save
            File.WriteAllText(@".\backup\json\locsaveedit.json", JsonConvert.SerializeObject(JObject.Parse(readfile), Formatting.Indented));
            //File.WriteAllText(@".\backup\json\locsaveedit.json", JsonConvert.SerializeObject(JObject.Parse(json), Formatting.Indented));
        }
        private void AddLoc(string name, string readpath, string writepath)//string jsona, string jsonb)
        {
            //Adds a location to a save file in the writepath
            //INPUT is jsonA.json and jsonB.json

            record = -1;

            string jsona = File.ReadAllText(readpath);
            var ja = JObject.Parse(jsona);
            var readfile = JsonConvert.SerializeObject(ja, Formatting.None);// Formatting.Indented);
            //File.WriteAllText(@".\backup\json\nlg_src.json", readfile);

            var nmsa = Nms.FromJson(readfile);

            //var nlgrecord = JsonConvert.SerializeObject(nmsa.The6F.NlG[record], Formatting.Indented);
            //textBox10.Text = nlgrecord;

            string jsonb = File.ReadAllText(writepath);
            var jb = JObject.Parse(jsonb);
            var writefile = JsonConvert.SerializeObject(jb, Formatting.None);// Formatting.Indented);
            //File.WriteAllText(@".\backup\json\nlg_dest.json", writefile);

            var nmsb = Nms.FromJson(writefile);

            var orignlgstr = JsonConvert.SerializeObject(nmsb.The6F.NlG, Formatting.None);// Formatting.Indented);
            //File.WriteAllText(@".\backup\json\nlg_orig.json", orignlgstr);

            for (int i = 0; i < nmsa.The6F.NlG.Length; i++)
            {
                if (nmsa.The6F.NlG[i].NKm == name)
                {
                    record = i;
                }
            }
            for (int i = 0; i < nmsb.The6F.NlG.Length; i++)
            {
                if (nmsb.The6F.NlG[i].NKm == nmsa.The6F.NlG[record].NKm)
                {
                    MessageBox.Show("Destination Already Contains Location!", "Alert", MessageBoxButtons.OK);
                    return;
                }
            }
            if (record == -1)
            {
                MessageBox.Show("Record not found!", "Alert", MessageBoxButtons.OK);
                return;
            }

            var newnlg = nmsb.The6F.NlG.Append(nmsa.The6F.NlG[record]);
            string newnlgstr = JsonConvert.SerializeObject(newnlg, Formatting.None);
            //File.WriteAllText(@".\backup\json\nlg_new.json", newnlgstr);
            //textBox11.Text = JsonConvert.SerializeObject(newnlg, Formatting.Indented);

            //Replace the old NlG with the new NlG
            //jsonb = jsonb.Replace(orignlgstr, newnlgstr);
            //File.WriteAllText(@".\backup\json\nlg_edit.json", JsonConvert.SerializeObject(JObject.Parse(jsonb), Formatting.Indented));
            writefile = writefile.Replace(orignlgstr, newnlgstr);
            //File.WriteAllText(@".\backup\json\nlg_edit.json", JsonConvert.SerializeObject(JObject.Parse(writefile), Formatting.Indented));

            /*
            if (nmsa.The6F.NlG[record].IAf.Contains("Base"))
            {
                AddBase(nmsa.The6F.NlG[record].NKm, readfile, writefile);
                return;
            }
            */
            
            //File.WriteAllText(@".\backup\json\locsaveedit.json", JsonConvert.SerializeObject(JObject.Parse(jsonb), Formatting.Indented));            
            File.WriteAllText(@".\backup\json\locsaveedit.json", JsonConvert.SerializeObject(JObject.Parse(writefile), Formatting.Indented));
        }
        private void AddLoc(int record, string readpath, string writepath)
        {
            //INPUT is jsonA.json or jsonB.json
            //Adds a certain record from readpath to writepath if record is known

            string jsona = File.ReadAllText(readpath);
            var ja = JObject.Parse(jsona);
            var readfile = JsonConvert.SerializeObject(ja, Formatting.None);// Formatting.Indented);
            //File.WriteAllText(@".\backup\json\nlg_src.json", readfile);

            var nmsa = Nms.FromJson(readfile);

            //var nlgrecord = JsonConvert.SerializeObject(nmsa.The6F.NlG[record], Formatting.Indented);
            //textBox10.Text = nlgrecord;

            string jsonb = File.ReadAllText(writepath);
            var jb = JObject.Parse(jsonb);
            var writefile = JsonConvert.SerializeObject(jb, Formatting.None);// Formatting.Indented);
            //File.WriteAllText(@".\backup\json\nlg_dest.json", writefile);

            var nmsb = Nms.FromJson(writefile);

            var orignlgstr = JsonConvert.SerializeObject(nmsb.The6F.NlG, Formatting.None);// Formatting.Indented);
            //File.WriteAllText(@".\backup\json\nlg_orig.json", orignlgstr);

            for (int i = 0; i < nmsb.The6F.NlG.Length; i++)
            {
                if (JsonConvert.SerializeObject(nmsb.The6F.NlG[i]) == JsonConvert.SerializeObject(nmsa.The6F.NlG[record]))
                {
                    MessageBox.Show("Destination Already Contains Location!", "Alert", MessageBoxButtons.OK);
                    return;
                }
            }

            var newnlg = nmsb.The6F.NlG.Append(nmsa.The6F.NlG[record]);
            string newnlgstr = JsonConvert.SerializeObject(newnlg, Formatting.None);// Formatting.Indented);
            //File.WriteAllText(@".\backup\json\nlg_new.json", newnlgstr);
            //textBox11.Text = JsonConvert.SerializeObject(newnlg, Formatting.Indented);

            //Replace the old NlG with the new NlG
            //jsonb = jsonb.Replace(orignlgstr, newnlgstr);
            //File.WriteAllText(@".\backup\json\nlg_edit.json", JsonConvert.SerializeObject(JObject.Parse(jsonb), Formatting.Indented));
            writefile = writefile.Replace(orignlgstr, newnlgstr);
            //File.WriteAllText(@".\backup\json\nlg_edit.json", JsonConvert.SerializeObject(JObject.Parse(writefile), Formatting.Indented));

            /*
            if (nmsa.The6F.NlG[record].IAf.Contains("Base"))
            {
                AddBase(nmsa.The6F.NlG[record].NKm, jsona, jsonb);
                return;
            }
            */
            
            //File.WriteAllText(@".\backup\json\locsaveedit.json", JsonConvert.SerializeObject(JObject.Parse(jsonb), Formatting.Indented));            
            File.WriteAllText(@".\backup\json\locsaveedit.json", JsonConvert.SerializeObject(JObject.Parse(writefile), Formatting.Indented));
        }
        private void AddBase(string basename, string readfile, string writefile) //string jsona, string jsonb)
        {
            //INPUT is 

            //var ja = JObject.Parse(jsona);
            //var readfile = JsonConvert.SerializeObject(ja, Formatting.None);// Formatting.Indented);
            //File.WriteAllText(@".\backup\json\F0_src.json", readfile);

            var nmsa = Nms.FromJson(readfile);

            //var nlgrecord = JsonConvert.SerializeObject(nmsa.The6F.NlG[record], Formatting.Indented);
            //textBox10.Text = nlgrecord;

            //var jb = JObject.Parse(jsonb);
            //var writefile = JsonConvert.SerializeObject(jb, Formatting.None);// Formatting.Indented);
            //File.WriteAllText(@".\backup\json\F0_dest.json", writefile);

            var nmsb = Nms.FromJson(writefile);

            var origF0str = JsonConvert.SerializeObject(nmsb.The6F.F0, Formatting.None);// Formatting.Indented);
            //File.WriteAllText(@".\backup\json\F0_orig.json", origF0str);
            for (int i = 0; i < nmsb.The6F.F0.Length; i++)
            {
                if (nmsb.The6F.F0[i].NKm == basename)
                {
                    MessageBox.Show("Destination Already Contains Location!", "Alert", MessageBoxButtons.OK);
                    return;
                }
            }
            for (int i = 0; i < nmsa.The6F.F0.Length; i++)
            {
                if (nmsa.The6F.F0[i].NKm == basename)
                {
                    record = i;
                }                
            }

            var newF0 = nmsb.The6F.F0.Append(nmsa.The6F.F0[record]);
            string newF0str = JsonConvert.SerializeObject(newF0, Formatting.None);// Formatting.Indented);
            //File.WriteAllText(@".\backup\json\F0_new.json", newF0str);
            //textBox11.Text = JsonConvert.SerializeObject(newnlg, Formatting.Indented);

            //jsonb = jsonb.Replace(origF0str, newF0str);
            //File.WriteAllText(@".\backup\json\F0_edit.json", JsonConvert.SerializeObject(JObject.Parse(jsonb), Formatting.Indented));
            //File.WriteAllText(@".\backup\json\locsaveedit.json", JsonConvert.SerializeObject(JObject.Parse(jsonb), Formatting.Indented));

            writefile = writefile.Replace(origF0str, newF0str);
            //File.WriteAllText(@".\backup\json\F0_edit.json", JsonConvert.SerializeObject(JObject.Parse(writefile), Formatting.Indented));
            File.WriteAllText(@".\backup\json\locsaveedit.json", JsonConvert.SerializeObject(JObject.Parse(writefile), Formatting.Indented));
        }
        private void GetLoc(string loc, List<string> srclist, List<string> destlist)
        {
            string jsona = File.ReadAllText(@".\backup\json\locsaveA.json");
            string locpatterna = "\"nlG\":.*?\"=3B\":";
            Regex myRegex1a = new Regex(locpatterna, RegexOptions.Singleline);
            Match m1a = myRegex1a.Match(jsona);
            string locationsa = m1a.ToString();
            //string locations = m1.Result(m1.ToString()).Replace("\"=3B\":", "");
            //AppendLine(textBox10, locationsa);

            Regex myRegex2a = new Regex("\"yhJ\":.*?\"tww\":", RegexOptions.Singleline);            
            MatchCollection m2a = myRegex2a.Matches(locationsa);

            AppendLine(textBox7, "Count is: " + m2a.Count);            
            
            foreach (Match match in m2a)
            {
                string m = match.Result(match.ToString()).Replace(match.ToString(), "      {\r\n        " + match.ToString() + " false\r\n      }");
                srclist.Add(m);
                
                //AppendLine(textBox11, "**********************");
                //AppendLine(textBox11, m.ToString());
            }
            
            foreach (string item in srclist)
            {
                if (item.Contains(loc))
                {
                    //AppendLine(textBox11, "NEW: \r\n" + item);
                    LocName = item;
                }
            }

            string jsonb = File.ReadAllText(@".\backup\json\locsaveB.json");
            string locpatternb = "\"nlG\":.*?\"=3B\":";
            Regex myRegex1b = new Regex(locpatternb, RegexOptions.Singleline);
            Match m1b = myRegex1b.Match(jsonb);
            string locationsb = m1b.ToString();
            //string locations = m1.Result(m1.ToString()).Replace("\"=3B\":", "");
            //AppendLine(textBox10, locationsb);

            Regex myRegex2b = new Regex("\"yhJ\":.*?\"tww\":", RegexOptions.Singleline);
            MatchCollection m2b = myRegex2b.Matches(locationsb);

            AppendLine(textBox7, "Count is: " + m2b.Count);

            foreach (Match match in m2b)
            {
                string m = match.Result(match.ToString()).Replace(match.ToString(), "      {\r\n        " + match.ToString() + " false\r\n      }");
                destlist.Add(m);

                //AppendLine(textBox11, "**********************");
                //AppendLine(textBox11, m.ToString());
            }

            string newlocations = locationsb.Replace(destlist[0], destlist[0] + ",\r\n" + LocName);
            //AppendLine(textBox12, newlocations);

            jsonb = Regex.Replace(jsonb, locpatternb, newlocations, RegexOptions.Singleline);
            File.WriteAllText(@".\backup\json\locsaveedit.json", jsonb);
        }
        private void DelLoc(string loc, List<string> list)
        {
            string json = File.ReadAllText(@".\backup\json\locsaveA.json");
            string locpattern = "\"nlG\":.*?\"=3B\":";
            Regex myRegex1 = new Regex(locpattern, RegexOptions.Singleline);
            Match m1 = myRegex1.Match(json);
            string locations = m1.ToString();
            //string locations = m1.Result(m1.ToString()).Replace("\"=3B\":", "");
            //AppendLine(textBox10, locations);

            Regex myRegex2 = new Regex("\"yhJ\":.*?\"tww\":", RegexOptions.Singleline);
            MatchCollection m2 = myRegex2.Matches(locations);

            AppendLine(textBox7, "Count is: " + m2.Count);

            foreach (Match match in m2)
            {
                string m = match.Result(match.ToString()).Replace(match.ToString(), ",\r\n      {\r\n        " + match.ToString() + " false\r\n      }");
                list.Add(m);

                //AppendLine(textBox11, "**********************");
                //AppendLine(textBox11, m.ToString());
            }

            foreach (string item in list)
            {
                if (item.Contains(loc))
                {
                    //AppendLine(textBox11, "Del: \r\n" + item);
                    //list.Remove(item);
                    LocName = item;
                }
            }
            //string newlocations = locations.Replace(list[0], list[0] + ",\r\n" + i);
            int index = list.IndexOf(LocName);
            string newlocations = locations.Replace(list[index], "");
            //AppendLine(textBox12, newlocations);

            json = Regex.Replace(json, locpattern, newlocations, RegexOptions.Singleline);
            File.WriteAllText(@".\backup\json\locsaveedit.json", json);            
        }
        private static bool Validate(int slot, string path, string loc)
        {
            if (slot < 1 || slot > 5 )
            {
                return false;
            }
            if (!File.Exists(path))
            {
                return false;
            }
            if (loc == "" || loc == null)
            {
                return false;
            }

            return true;
        }        
        private async void Button7_Click(object sender, EventArgs e)
        {
            string locname = listBox1.GetItemText(listBox1.SelectedItem);

            if (locname == "")
            {
                MessageBox.Show("Something Went Wrong", "Error", MessageBoxButtons.OK);
                return;
            }
            if (!Validate(saveslota, hgFilePathA, LocToMove))
            {
                MessageBox.Show("Something Went Wrong", "Error", MessageBoxButtons.OK);
                return;
            }
            if (!Validate(saveslotb, hgFilePathB, LocToMove))
            {
                MessageBox.Show("Something Went Wrong", "Error", MessageBoxButtons.OK);
                return;
            }
            if (listBox2.Items.Contains(listBox1.SelectedItem))
            {
                MessageBox.Show("Duplicate", "Error", MessageBoxButtons.OK);
                return;
            }
            /*
            BackUpSaveSlot(textBox7, saveslotb, false, hgFilePathB);
            BackUpSaveSlot(textBox7, saveslota, false, hgFilePathA);
            DecryptSave(saveslota, hgFilePathA, "A");
            DecryptSave(saveslotb, hgFilePathB, "B");
            GetLoc(LocToMove, ListA, ListB);
            RunEncrypt(progressBar1, saveslotb);
            progressBar1.Value = 0;
            */
            fileSystemWatcher1.EnableRaisingEvents = false;
            progressBar1.Invoke((Action)(() => progressBar1.Value = 5));

            BackUpSaveSlot(textBox7, saveslota, false, hgFilePathA);            
            progressBar1.Invoke((Action)(() => progressBar1.Value = 25));

            await Task.Delay(200);
            BackUpSaveSlot(textBox7, saveslotb, false, hgFilePathB);
            progressBar1.Invoke((Action)(() => progressBar1.Value = 35));

            
            await Task.Delay(200);
            DecryptSave(saveslota, hgFilePathA, "A");
            progressBar1.Invoke((Action)(() => progressBar1.Value = 45));
            
            await Task.Delay(200);
            DecryptSave(saveslotb, hgFilePathB, "B");
            progressBar1.Invoke((Action)(() => progressBar1.Value = 70));
            

            //AddLoc(listBox1.SelectedIndex, hgFilePathA, hgFilePathB);
            //AddLoc(listBox1.SelectedIndex, @".\backup\json\locsaveA.json", @".\backup\json\locsaveB.json");

            await Task.Delay(200);
            AppendLine(textBox7, "Location: " + locname + " Source: " + Path.GetFileName(hgFilePathA) + " Destination: " + Path.GetFileName(hgFilePathB));
            AddLoc(locname, @".\backup\json\locsaveA.json", @".\backup\json\locsaveB.json");
            //AddLoc(locname, jsonA, jsonB);
            progressBar1.Invoke((Action)(() => progressBar1.Value = 80));            

            await Task.Delay(500);
            SetWriteTime(hgFilePathB);
            EncryptSave(progressBar1, saveslotb);
            //RunEncrypt(progressBar1, saveslotb);

            //button2.PerformClick();
            //button4.PerformClick();
            ReloadSave("AB", true);

            progressBar1.Value = 0;

            fileSystemWatcher1.EnableRaisingEvents = true;
        }
        private async void Button8_Click(object sender, EventArgs e)
        {
            fileSystemWatcher1.EnableRaisingEvents = false;

            string locname = listBox2.GetItemText(listBox2.SelectedItem);

            if ( locname == "")
            {
                MessageBox.Show("Something Went Wrong", "Error", MessageBoxButtons.OK);
                return;
            }
            if (!Validate(saveslota, hgFilePathA, LocToMove))
            {
                MessageBox.Show("Something Went Wrong", "Error", MessageBoxButtons.OK);
                return;
            }
            if (!Validate(saveslotb, hgFilePathB, LocToMove))
            {
                MessageBox.Show("Something Went Wrong", "Error", MessageBoxButtons.OK);
                return;
            }
            if (listBox1.Items.Contains(listBox2.SelectedItem))
            {
                MessageBox.Show("Duplicate", "Error", MessageBoxButtons.OK);
                return;
            }
            /*
            BackUpSaveSlot(textBox7, saveslota, false, hgFilePathA);
            BackUpSaveSlot(textBox7, saveslotb, false, hgFilePathB);
            DecryptSave(saveslota, hgFilePathA, "B");
            DecryptSave(saveslotb, hgFilePathB, "A");
            GetLoc(LocToMove, ListB, ListA);
            RunEncrypt(progressBar1, saveslota);
            progressBar1.Value = 0;
            */
            progressBar1.Invoke((Action)(() => progressBar1.Value = 5));
                        
            BackUpSaveSlot(textBox7, saveslota, false, hgFilePathA);
            progressBar1.Invoke((Action)(() => progressBar1.Value = 25));

            await Task.Delay(200);
            BackUpSaveSlot(textBox7, saveslotb, false, hgFilePathB);
            progressBar1.Invoke((Action)(() => progressBar1.Value = 35));

            
            await Task.Delay(200);
            DecryptSave(saveslota, hgFilePathA, "A");
            progressBar1.Invoke((Action)(() => progressBar1.Value = 45));

            await Task.Delay(200);
            DecryptSave(saveslotb, hgFilePathB, "B");
            progressBar1.Invoke((Action)(() => progressBar1.Value = 70));
            

            //AddLoc(listBox2.SelectedIndex, hgFilePathB, hgFilePathA);
            //AddLoc(listBox2.SelectedIndex, @".\backup\json\locsaveB.json", @".\backup\json\locsaveA.json");

            await Task.Delay(200);
            AppendLine(textBox7, "Location: " + locname + " Source: " + Path.GetFileName(hgFilePathB) + " Destination: " + Path.GetFileName(hgFilePathA));
            //AddLoc(locname, hgFilePathB, hgFilePathA);
            //AddLoc(locname, jsonB, jsonA);
            AddLoc(locname, @".\backup\json\locsaveB.json", @".\backup\json\locsaveA.json");
            progressBar1.Invoke((Action)(() => progressBar1.Value = 80));

            await Task.Delay(500);
            SetWriteTime(hgFilePathA);
            EncryptSave(progressBar1, saveslota);
            //RunEncrypt(progressBar1, saveslota);

            //button4.PerformClick();
            //button2.PerformClick();
            ReloadSave("AB", true);

            progressBar1.Value = 0;

            fileSystemWatcher1.EnableRaisingEvents = true;
        }        
        private async void FileSystemWatcher1_Changed(object sender, FileSystemEventArgs e)
        {
            //Watch for changes in hg files
            List<string> list = new List<string>();

            foreach (KeyValuePair<int, string> item in comboBox1.Items)
            {
                list.Add(item.Value);
            }
            foreach (KeyValuePair<int, string> item in comboBox3.Items)
            {
                list.Add(item.Value);
            }

            if (list.Contains(e.Name))
            {
                lock (_changedFiles)
                {
                    if (_changedFiles.Contains(e.FullPath))
                    {
                        return;
                    }
                    _changedFiles.Add(e.FullPath);
                }

                //if changes detected, show form7 files changed externally
                Form7 f7 = new Form7();
                f7.ShowDialog();

                if (f7.SaveChanged == true)
                {
                    var selecteda = comboBox2.SelectedItem;
                    var selectedb = comboBox4.SelectedItem;

                    ClearA();
                    LoadCmbxA();
                    comboBox2.SelectedItem = selecteda;
                    ComboBox2_SelectionChangeCommitted(this, new EventArgs());

                    await Task.Delay(300);

                    ClearB();                    
                    LoadCmbxB();
                    comboBox4.SelectedItem = selectedb;
                    ComboBox4_SelectionChangeCommitted(this, new EventArgs());

                    
                    //ReloadSave("AB", false);

                    /*
                    progressBar1.Invoke((Action)(() => progressBar1.Value = 5));

                    //ClearAll();
                    ClearA();
                    ClearB();
                    progressBar1.Invoke((Action)(() => progressBar1.Value = 10));

                    LoadCmbxA();
                    await Task.Delay(200);
                    progressBar1.Invoke((Action)(() => progressBar1.Value = 25));

                    LoadCmbxB();
                    await Task.Delay(200);
                    progressBar1.Invoke((Action)(() => progressBar1.Value = 50));

                    comboBox2.SelectedItem = selecteda;
                    await Task.Delay(200);
                    progressBar1.Invoke((Action)(() => progressBar1.Value = 75));

                    comboBox4.SelectedItem = selectedb;
                    await Task.Delay(200);
                    progressBar1.Invoke((Action)(() => progressBar1.Value = 80));

                    ComboBox2_SelectionChangeCommitted(this, new EventArgs());
                    await Task.Delay(200);
                    progressBar1.Invoke((Action)(() => progressBar1.Value = 90));

                    ComboBox4_SelectionChangeCommitted(this, new EventArgs());
                    await Task.Delay(200);
                    progressBar1.Invoke((Action)(() => progressBar1.Value = 100));

                    progressBar1.Value = 0;
                    */
                }
                else
                {
                    AppendLine(textBox7, "Not Viewing the latest save!");
                }

                System.Timers.Timer timer = new System.Timers.Timer(1000) { AutoReset = false };
                timer.Elapsed += (timerElapsedSender, timerElapsedArgs) =>
                {
                    lock (_changedFiles)
                    {
                        _changedFiles.Remove(e.FullPath);
                    }
                };
                timer.Start();
            }
        }
        private void ClearA()
        {
            textBox1.Clear();
            textBox3.Clear();
            textBox5.Clear();
            textBox8.Clear();

            textBox10.Clear();
            textBox11.Clear();
            textBox12.Clear();
            textBox13.Clear();

            listBox1.Items.Clear();

            record = -1;
            jsonA = "";
        }
        private void ClearB()
        {
            textBox2.Clear();
            textBox9.Clear();
            textBox4.Clear();
            textBox6.Clear();

            textBox10.Clear();
            textBox11.Clear();
            textBox12.Clear();
            textBox13.Clear();

            listBox2.Items.Clear();

            record = -1;
            jsonB = "";
        }
        private void ClearAll()
        {
            comboBox1.DataSource = null;
            comboBox2.DataSource = null;
            comboBox3.DataSource = null;
            comboBox4.DataSource = null;

            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();
            textBox6.Clear();
            textBox7.Clear();
            textBox8.Clear();
            textBox9.Clear();
            textBox10.Clear();
            textBox11.Clear();
            textBox12.Clear();
            textBox13.Clear();

            listBox1.Items.Clear();
            listBox2.Items.Clear();

            record = -1;

            hgFilePathA = "";
            hgFilePathB = "";
            hgFileDir = "";

            jsonA = "";
            jsonB = "";
        }
        private void Button2_Click(object sender, EventArgs e)
        {
            ReloadSave("A", true);

            /*
            //Load newest
            var selected = comboBox2.SelectedItem;
            ClearAll();
            LoadCmbxA();
            comboBox2.SelectedItem = selected;
            ComboBox2_SelectionChangeCommitted(this, new EventArgs());
            */

            /*
            //Reload save button
            string selected = this.comboBox1.GetItemText(this.comboBox1.SelectedItem);
            if (selected != "")
            {
                ClearA();
                AppendLine(textBox7, "Loading Save File...");
                GetSaveFileA(selected);
                Loadlsb1A();
                //LoadBaselsbx();
                //GetPlayerCoord();
                //LoadTxt();
                AppendLine(textBox7, "Save File Reloaded.");
            }
            else
            {
                MessageBox.Show("No Save Slot Selected!", "Alert");
            }
            */
        }
        private void Button4_Click(object sender, EventArgs e)
        {
            ReloadSave("B", true);

            /*
            //Load Newest
            var selected = comboBox4.SelectedItem;
            ClearAll();
            LoadCmbxB();
            comboBox4.SelectedItem = selected;
            ComboBox4_SelectionChangeCommitted(this, new EventArgs());
            */
            
            /*
            //Reload save button
            string selected = this.comboBox3.GetItemText(this.comboBox3.SelectedItem);
            if (selected != "")
            {
                ClearB();
                AppendLine(textBox7, "Loading Save File...");
                GetSaveFileB(selected);
                Loadlsb1B();
                //LoadBaselsbx();
                //GetPlayerCoord();
                //LoadTxt();
                AppendLine(textBox7, "Save File Reloaded.");
            }
            else
            {
                MessageBox.Show("No Save Slot Selected!", "Alert");
            }
            */
        }
        private void ReloadSave(string saveside, bool check)
        {
            //Reload save
            string selecteda = this.comboBox1.GetItemText(this.comboBox1.SelectedItem);
            string selectedb = this.comboBox3.GetItemText(this.comboBox3.SelectedItem);
                        
            switch (saveside)
            {
                case "A":
                    if (check)
                    {
                        if (selecteda == "")
                        {
                            MessageBox.Show("No Save Slot Selected!", "Alert");
                            return;
                        }
                    }
                    ClearA();
                    AppendLine(textBox7, "Loading Save File...");                    
                    GetSaveFileA(selecteda);
                    Loadlsb1A();
                    //LoadBaselsbx();
                    //GetPlayerCoord();
                    //LoadTxt();
                    AppendLine(textBox7, "Save File Reloaded.");
                   break;

                case "B":
                    if (check)
                    {
                        if (selectedb == "")
                        {
                            MessageBox.Show("No Save Slot Selected!", "Alert");
                            return;
                        }
                    }
                    ClearB();
                    AppendLine(textBox7, "Loading Save File...");
                    GetSaveFileB(selectedb);
                    Loadlsb1B();
                    //LoadBaselsbx();
                    //GetPlayerCoord();
                    //LoadTxt();
                    AppendLine(textBox7, "Save File Reloaded.");
                    break;

                case "AB":
                    if (check)
                    {
                        if (selecteda == "" || selectedb == "")
                        {
                            MessageBox.Show("No Save Slot Selected!", "Alert");
                            return;
                        }
                    }
                    ClearA();
                    AppendLine(textBox7, "Loading Save Files A...");
                    GetSaveFileA(selecteda);
                    Loadlsb1A();                    

                    ClearB();
                    AppendLine(textBox7, "Loading Save Files B...");
                    GetSaveFileB(selectedb);                    
                    Loadlsb1B();

                    AppendLine(textBox7, "Save Files Reloaded.");
                    break;
            }            
        }
        private async void DeleteLocationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string locname = listBox1.GetItemText(listBox1.SelectedItem);

            if (locname == "")
            {
                MessageBox.Show("Something Went Wrong", "Error", MessageBoxButtons.OK);
                return;
            }

            fileSystemWatcher1.EnableRaisingEvents = false;
            progressBar1.Invoke((Action)(() => progressBar1.Value = 5));

            await Task.Delay(200);
            BackUpSaveSlot(textBox7, saveslota, false, hgFilePathA);            
            progressBar1.Invoke((Action)(() => progressBar1.Value = 25));

            
            await Task.Delay(200);
            DecryptSave(saveslota, hgFilePathA, "A");
            progressBar1.Invoke((Action)(() => progressBar1.Value = 45));
            

            //DelLoc(LocToMove, ListA);
            //DelLoc(listBox1.SelectedIndex, @".\backup\json\locsaveA.json");
            //DelLoc(listBox1.SelectedIndex, hgFilePathA);

            await Task.Delay(200);
            AppendLine(textBox7, "Delete location: " + locname + " from save file: " + Path.GetFileName(hgFilePathA));
            //DelLoc(locname, hgFilePathA);
            //DelLoc(locname, jsonA);
            DelLoc(locname, @".\backup\json\locsaveA.json");
            progressBar1.Invoke((Action)(() => progressBar1.Value = 65));

            await Task.Delay(500);
            EncryptSave(progressBar1, saveslota);
            //RunEncrypt(progressBar1, saveslota);

            //button4.PerformClick();
            //button2.PerformClick();

            await Task.Delay(300);
            ReloadSave("A", false);

            progressBar1.Value = 0;

            fileSystemWatcher1.EnableRaisingEvents = true;
        }
        private async void DeleteLocationToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            string locname = listBox2.GetItemText(listBox2.SelectedItem);

            if (locname == "")
            {
                MessageBox.Show("Something Went Wrong", "Error", MessageBoxButtons.OK);
                return;
            }

            fileSystemWatcher1.EnableRaisingEvents = false;
            progressBar1.Invoke((Action)(() => progressBar1.Value = 5));

            await Task.Delay(200);
            BackUpSaveSlot(textBox7, saveslotb, false, hgFilePathB);            
            progressBar1.Invoke((Action)(() => progressBar1.Value = 25));

            
            DecryptSave(saveslotb, hgFilePathB, "B");
            await Task.Delay(200);
            progressBar1.Invoke((Action)(() => progressBar1.Value = 45));
            

            //DelLoc(LocToMove, ListB);
            //DelLoc(listBox2.SelectedIndex, @".\backup\json\locsaveB.json");
            //DelLoc(listBox2.SelectedIndex, hgFilePathB);

            await Task.Delay(200);
            AppendLine(textBox7, "Delete location: " + locname + " from save file: " + Path.GetFileName(hgFilePathB));
            //DelLoc(locname, hgFilePathB);
            //DelLoc(locname, jsonB);
            DelLoc(locname, @".\backup\json\locsaveB.json");
            progressBar1.Invoke((Action)(() => progressBar1.Value = 65));

            await Task.Delay(500);
            EncryptSave(progressBar1, saveslotb);
            //RunEncrypt(progressBar1, saveslotb);

            //button4.PerformClick();
            //button2.PerformClick();

            await Task.Delay(300);
            ReloadSave("B", false);

            progressBar1.Value = 0;

            fileSystemWatcher1.EnableRaisingEvents = true;
        }
        private void Button1_Click(object sender, EventArgs e)
        {
            //Backup save slot A
            if (comboBox1.GetItemText(comboBox1.SelectedItem) != "")
                BackUpSaveSlot(textBox7, saveslota, true, hgFilePathA);
            else
                MessageBox.Show("Please select a save slot!", "Alert");
        }
        private void Button3_Click(object sender, EventArgs e)
        {
            //Backup save slot B
            if (comboBox3.GetItemText(comboBox3.SelectedItem) != "")
                BackUpSaveSlot(textBox7, saveslotb, true, hgFilePathB);
            else
                MessageBox.Show("Please select a save slot!", "Alert");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button9_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            textBox7.Clear();

            string jsono = File.ReadAllText(hgFilePathA);
            var nms = Nms.FromJson(jsono);

            /*
            var fo = JsonConvert.SerializeObject(nms.The6F.QQp, Formatting.Indented);
            textBox10.Text = fo;
            nms.The6F.QQp = 99;
            textBox11.Text = JsonConvert.SerializeObject(nms.The6F, Formatting.Indented);

            var a = JsonConvert.SerializeObject(nms.The6F.F0, Formatting.Indented);
            var b = JsonConvert.SerializeObject(nms.FDu.Eto.OsQ.FB, Formatting.Indented);
            string json = JsonConvert.SerializeObject(nms.The6F.NlG[0], Formatting.Indented);
            File.WriteAllText(@".\backup\jsonO.json", foi);
            
            textBox11.Text = a;
            textBox12.Text = json;

            var nms = Nms.FromJson(jsonA);
            List<string> lst = new List<string>();
            List<string> lstb = new List<string>();
            */

            try
            {
                //Persistent Bases
                ListViewItem.ListViewSubItem[] subItems;
                ListViewItem item = null;
                //var basediscov = JsonConvert.SerializeObject(nms.The6F.F0, Formatting.Indented);
                for (int i = 0; i < nms.The6F.F0.Length; i++)
                {
                    string name = nms.The6F.F0[i].NKm;
                    if (name == "")
                    {
                        continue;
                    }

                    //lstb.Add(nms.The6F.F0[i].OZw);

                    item = new ListViewItem(nms.The6F.F0[i].OZw, 1);
                    CalculateLongHex(nms.The6F.F0[i].OZw, false);
                    string[] value = GalacticCoord2.Replace(" ", "").Split(':');
                    string A = value[0].Trim();
                    string B = value[1].Trim();
                    string C = value[2].Trim();
                    string D = value[3].Trim();
                    
                    //Validate Coordinates
                    if (ValidateCoord(A, B, C, D))
                    {
                        MessageBox.Show("Invalid Coordinates! Out of Range!", "Alert");
                        //Clear();
                        AppendLine(textBox7, "Invalid Coordinates!");
                        continue;
                    }
                    GalacticToPortal(Planet, A, B, C, D, false);

                    subItems = new ListViewItem.ListViewSubItem[]
                    { new ListViewItem.ListViewSubItem(item, PortalCode), new ListViewItem.ListViewSubItem(item, GalacticCoord2), new ListViewItem.ListViewSubItem(item, name) };

                    item.SubItems.AddRange(subItems);
                    listView1.Items.Add(item);
                }              
                listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
                //listBox4.DataSource = lstb;

                //Discoveries
                for (int i = 0; i < nms.FDu.Eto.OsQ.FB.Length; i++)
                {
                    //var discov = JsonConvert.SerializeObject(nms.FDu.Eto.OsQ.FB[i].The8P3.The5L6, Formatting.Indented);
                    //discov = discov.Replace("\"", "");
                    //listBox1.Items.Add(discd);
                    if (nms.FDu.Eto.OsQ.FB[i].The8P3.Dn == "Planet")
                    {
                        //lst.Add(discov);

                        item = new ListViewItem(nms.FDu.Eto.OsQ.FB[i].The8P3.The5L6, 1);
                        CalculateLongHex(nms.FDu.Eto.OsQ.FB[i].The8P3.The5L6, false);
                        string[] value = GalacticCoord2.Replace(" ", "").Split(':');
                        string A = value[0].Trim();
                        string B = value[1].Trim();
                        string C = value[2].Trim();
                        string D = value[3].Trim();

                        //Validate Coordinates
                        if (ValidateCoord(A, B, C, D))
                        {
                            MessageBox.Show("Invalid Coordinates! Out of Range!", "Alert");
                            //Clear();
                            AppendLine(textBox7, "Invalid Coordinates!");
                            continue;
                        }
                        GalacticToPortal(Planet, A, B, C, D, false);

                        subItems = new ListViewItem.ListViewSubItem[]
                        { new ListViewItem.ListViewSubItem(item, PortalCode),
                        new ListViewItem.ListViewSubItem(item, GalacticCoord2),
                        new ListViewItem.ListViewSubItem(item, nms.FDu.Eto.OsQ.FB[i].The8P3.Dn + " ( " + i + " ) ")};

                        item.SubItems.AddRange(subItems);
                        listView1.Items.Add(item);
                    }
                }
                listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
                //listBox3.DataSource = lst;

                //Terminus
                for (int i = 0; i < nms.The6F.NlG.Length; i++)
                {
                    //var discov = JsonConvert.SerializeObject(nms.The6F.NlG[i].NKm, Formatting.Indented);
                    //discov = discov.Replace("\"", "");
                    //listBox1.Items.Add(discd);
                    if (nms.The6F.NlG[i].NKm != "")
                    {
                        //lst.Add(discov);
                        Planet = Convert.ToInt32(nms.The6F.NlG[i].YhJ.OZw["jsv"]);
                        var X = Convert.ToInt32(nms.The6F.NlG[i].YhJ.OZw["dZj"]);
                        var Y = Convert.ToInt32(nms.The6F.NlG[i].YhJ.OZw["IyE"]);
                        var Z = Convert.ToInt32(nms.The6F.NlG[i].YhJ.OZw["uXE"]);
                        var SSI = Convert.ToInt32(nms.The6F.NlG[i].YhJ.OZw["vby"]);
                        VoxelToGalacticCoord(X, Y, Z, SSI, false);

                        item = new ListViewItem(nms.The6F.NlG[i].IAf, 1);
                        
                        string[] value = GalacticCoord.Replace(" ", "").Split(':');
                        string A = value[0].Trim();
                        string B = value[1].Trim();
                        string C = value[2].Trim();
                        string D = value[3].Trim();

                        //Validate Coordinates
                        if (ValidateCoord(A, B, C, D))
                        {
                            MessageBox.Show("Invalid Coordinates! Out of Range!", "Alert");
                            //Clear();
                            AppendLine(textBox7, "Invalid Coordinates!");
                            continue;
                        }
                        GalacticToPortal(Planet, A, B, C, D, false);

                        subItems = new ListViewItem.ListViewSubItem[]
                        { new ListViewItem.ListViewSubItem(item, PortalCode),
                        new ListViewItem.ListViewSubItem(item, GalacticCoord),
                        new ListViewItem.ListViewSubItem(item, nms.The6F.NlG[i].NKm)};

                        item.SubItems.AddRange(subItems);
                        listView1.Items.Add(item);
                    }
                }
                listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
                //listBox3.DataSource = lst;
            }
            catch
            {
                AppendLine(textBox7, "** ERROR: Failed find coordinates**");
                return;
            }
        }
        public static void Log(TextBox source, string value, bool log)
        {
            //My neat little textbox handler
            if (log)
            {
                if (source.Text.Length == 0)
                    source.Text = value;
                else
                    source.AppendText("\r\n" + value);
            }
        }
        private void ClearHx()
        {
            hxx = "";
            hxe = 0;
            GalacticCoord2 = "";
            GalacticCoord = "";
            Planet = 0;
            PortalCode = "";
        }
        private void VoxelToGalacticCoord(int X, int Y, int Z, int SSI, bool log)
        {
            //Voxel Coordinate to Galactic Coordinate
            if (log)
            {
                textBox7.Clear();
            }                

            //Note: iX, iY, iZ, iSSI already Convert.ToInt32(X) in JSONMap()
            //AppendLine(textBox7, "DEC: " + iX + " " + iY + " " + iZ);

            int dd1 = X + 2047;
            int dd2 = Y + 127;
            int dd3 = Z + 2047;
            Log(textBox7, "SHIFT: " + dd1 + " " + dd2 + " " + dd3, log);

            string g1 = dd1.ToString("X");
            string g2 = dd2.ToString("X");
            string g3 = dd3.ToString("X");
            string g4 = SSI.ToString("X");
            Log(textBox7, "Galactic HEX numbers: " + g1 + " " + g2 + " " + g3 + " " + g4, log);

            int ig1 = Convert.ToInt32(g1, 16); // X[HEX] to X[DEC]
            int ig2 = Convert.ToInt32(g2, 16); // Y[HEX] to Y[DEC]
            int ig3 = Convert.ToInt32(g3, 16); // Z[HEX] to Z[DEC]
            int ig4 = Convert.ToInt32(g4, 16); // SSI[HEX] to SSI[DEC]
            Log(textBox7, "Galactic DEC numbers: " + ig1 + " " + ig2 + " " + ig3 + " " + ig4, log);

            GalacticCoord = string.Format("{0:X4}:{1:X4}:{2:X4}:{3:X4}", ig1, ig2, ig3, ig4); //Format to 4 digit seperated by colon
            Log(textBox7, "*** Galactic Coordinates: " + GalacticCoord + " ***", log);
        }
        private void GalacticToPortal(int P, string X, string Y, string Z, string SSI, bool log)
        {
            //Galactic Coordinate to Portal Code
            PortalCode = "";

            int dec1 = Convert.ToInt32(X, 16); // X[HEX] to X[DEC]
            int dec2 = Convert.ToInt32(Y, 16); // Y[HEX] to X[DEC]
            int dec3 = Convert.ToInt32(Z, 16); // Z[HEX] to X[DEC]
            int dec4 = Convert.ToInt32(SSI, 16); // SSI[HEX] to SSI[DEC]
            Log(textBox7, "Galactic HEX to DEC: " + dec1.ToString() + " " + dec2.ToString() + " " + dec3.ToString() + " " + dec4, log);

            //string g4 = SSI.ToString("X");

            int dec5 = Convert.ToInt32("801", 16); // 801[HEX] to 801[DEC]
            int dec6 = Convert.ToInt32("81", 16); // 81[HEX] to 81[DEC]
            int dec7 = Convert.ToInt32("1000", 16); // 100[HEX] to 1000[DEC]
            int dec8 = Convert.ToInt32("100", 16); // 100[HEX] to 100[DEC]
            Log(textBox7, "Shift HEX to DEC: " + "801:" + dec5.ToString() + " 81:" + dec6.ToString() + " 1000:" + dec7.ToString() + " 100:" + dec8.ToString(), log);

            int calc1 = (dec1 + dec5) % dec7; // (X[DEC] + 801[DEC]) MOD (1000[DEC])
            int calc2 = (dec2 + dec6) % dec8; // (Y[DEC] + 81[DEC]) MOD (100[DEC])
            int calc3 = (dec3 + dec5) % dec7; // (Z[DEC] + 801[DEC]) MOD (1000[DEC])
            Log(textBox7, "Calculate Portal DEC: " + "X:" + calc1.ToString() + " Y:" + calc2.ToString() + " Z:" + calc3.ToString() + " SSI:" + dec4, log);

            string hexX = calc1.ToString("X"); //Calculated portal X[DEC] to X[HEX]
            string hexY = calc2.ToString("X"); //Calculated portal Y[DEC] to Y[HEX]
            string hexZ = calc3.ToString("X"); //Calculated portal Z[DEC] to Z[HEX]
            Log(textBox7, "Portal HEX numbers: " + "X:" + hexX + " Y:" + hexY + " Z:" + hexZ + " SSI:" + SSI, log);

            int ihexX = (Convert.ToInt32(hexX, 16) & 0xFFF); // X[HEX] to X[DEC] 3 digits
            int ihexY = (Convert.ToInt32(hexY, 16) & 0xFF); // Y[HEX] to Y[DEC] 2 digits
            int ihexZ = (Convert.ToInt32(hexZ, 16) & 0xFFF); // Z[HEX] to Z[DEC] 3 digits
            int ihexSSI = (Convert.ToInt32(SSI, 16) & 0xFFF); // SSI[HEX] to SSI[DEC] 3 digits

            PortalCode = string.Format(P + "{0:X3}{1:X2}{2:X3}{3:X3}", ihexSSI, ihexY, ihexZ, ihexX); // Format digits 0 3 2 3 3
            //[P][SSI][Y][Z][X] Portal Code
            Log(textBox7, "*** Portal Code: " + PortalCode + " ***", log);

            Planet = 0;
        }
        private void CalculateLongHex(string hx, bool log)
        {
            //PlanetNumber--SolarSystemIndex--GalaxyNumber--VoxelY--VoxelZ--VoxelX
            //4 bit--12 bit--8 bit--8 bit--12 bit--12 bit
            // 0x1 05C 00 FE 54B C32  4604758964091954  0431:007D:0D4A:005C  005CFE54BC32

            ClearHx();
            hxx = hx;
            if (!hx.StartsWith("0x"))
            {
                hxe = Convert.ToInt64(hx);
                Log(textBox7, "Long DEC: " + hxe, log); // Display Long DEC

                hxx = "0x" + hxe.ToString("X"); // Convert Long DEC to Long HEX
                Log(textBox7, "Long HEX: " + hxx, log);// Display Long HEX
            }
            else
            {
                hxe = Convert.ToInt64(hx, 16); // Convert Long HEX to DEC
                Log(textBox7, "Long DEC: " + hxe, log); // Display Long DEC
                Log(textBox7, "Long HEX: " + hxx, log); // Display Long HEX
            }

            string basehx = hxx;
            string b6 = basehx.Substring(basehx.Length - 3, 3);
            string b5 = basehx.Substring(basehx.Length - 6, 3);
            string b4 = basehx.Substring(basehx.Length - 8, 2);
            string b3 = basehx.Substring(basehx.Length - 10, 2);
            string b2 = basehx.Substring(basehx.Length - 13, 3);
            string b1 = basehx.Substring(basehx.Length - 16, 3);
            Log(textBox7, "Base Hex Split: " + b1 + " " + b2 + " " + b3 + " " + b4 + " " + b5 + " " + b6, log);
            Log(textBox7, "Base Hex id's: Planet #:" + b1 + " SSI:" + b2 + " Gal#:" + b3 + " Y:" + b4 + " Z:" + b5 + " X:" + b6, log);

            int dec1 = Convert.ToInt32("1000", 16); // 1000[HEX] to 1000[DEC]
            int dec2 = Convert.ToInt32("100", 16); // 100[HEX] to 100[DEC]
            int dec3 = Convert.ToInt32("7F", 16); // [HEX] to [DEC]
            int dec4 = Convert.ToInt32("7FF", 16); // [HEX] to [DEC]
            Log(textBox7, "SHIFT calc: 1000:" + dec1 + " 100:" + dec2 + " 7F:" + dec3 + " 7FF:" + dec4, log);

            //= BASE(MOD(HEX2DEC(Y) + HEX2DEC(7F), HEX2DEC(100)), 16, 4)
            int pidec = Convert.ToInt32(b1, 16); //HEX to DEC
            int ssidec = Convert.ToInt32(b2, 16);
            int galdec = Convert.ToInt32(b3, 16);
            int decY = Convert.ToInt32(b4, 16);
            int decZ = Convert.ToInt32(b5, 16);
            int decX = Convert.ToInt32(b6, 16);
            Log(textBox7, "Base Dec: Planet #:" + pidec + " SSI:" + ssidec + " Gal#:" + galdec + " Y:" + decY + " Z:" + decZ + " X:" + decX, log);

            int calc1 = (decX + dec4) % dec1; // (X[DEC] + 801[DEC]) MOD (1000[DEC])
            int calc2 = (decY + dec3) % dec2; // (Y[DEC] + 81[DEC]) MOD (100[DEC])
            int calc3 = (decZ + dec4) % dec1; // (Z[DEC] + 801[DEC]) MOD (1000[DEC])
            //AppendLine(textBox3, "1- X:" + calc1.ToString() + " Y:" + calc2.ToString() + " Z:" + calc3.ToString() + " SSI:" + ssidec);
            Log(textBox7, "Base Voxel Dec: Planet #:" + pidec + " SSI:" + ssidec + " Gal#:" + galdec + " Y:" + calc2.ToString() + " Z:" + calc3.ToString() + " X:" + calc1.ToString(), log);

            int shiftX = calc1 - 2047;
            int shiftY = calc2 - 127;
            int shiftZ = calc3 - 2047;
            //AppendLine(textBox3, "Voxel Coordinates: X:" + shiftX + " Y:" + shiftY + " Z:" + shiftZ + " SSI:" + ssidec);
            Log(textBox7, "Base Voxel: Planet #:" + pidec + " SSI:" + ssidec + " Gal#:" + galdec + " Y:" + shiftY + " Z:" + shiftZ + " X:" + shiftX, log);

            string hexX = calc1.ToString("X"); //Calculated portal X[DEC] to X[HEX]
            string hexY = calc2.ToString("X"); //Calculated portal Y[DEC] to Y[HEX]
            string hexZ = calc3.ToString("X"); //Calculated portal Z[DEC] to Z[HEX]
            //AppendLine(textBox7, "X:" + hexX + " Y:" + hexY + " Z:" + hexZ);

            int ihexX = (Convert.ToInt32(hexX, 16) & 0xFFFF); // X[HEX] to X[DEC] 3 digits
            int ihexY = (Convert.ToInt32(hexY, 16) & 0xFFFF); // Y[HEX] to Y[DEC] 2 digits
            int ihexZ = (Convert.ToInt32(hexZ, 16) & 0xFFFF); // Z[HEX] to Z[DEC] 3 digits
            //int ihexSSI = (Convert.ToInt32(ssidec, 16) & 0xFFFF); // SSI[HEX] to SSI[DEC] 3 digits

            //AppendLine(textBox14, "P: " + "X:" + hexX + " Y:" + hexY + " Z:" + hexZ + " SSI:" + ssidec);
            GalacticCoord2 = string.Format("{0:X4}:{1:X4}:{2:X4}:{3:X4}", ihexX, ihexY, ihexZ, ssidec & 0xFFFF); //Format to 4 digit seperated by colon
            Log(textBox7, "*** Galactic Coordinates: " + GalacticCoord2 + " ***", log);
            
            Planet = pidec;
        }
        private bool ValidateCoord(string A, string B, string C, string D)
        {
            bool x = Convert.ToInt32(A, 16) > 4096 || Convert.ToInt32(B, 16) > 255 || Convert.ToInt32(C, 16) > 4096 || Convert.ToInt32(D, 16) > 767;
            return x;
        }      
        private void GIndex()
        {
            //Main dictionary for galaxies
            galaxyDict = new Dictionary<string, string>();
            galaxyDict.Add(new KeyValuePair<string, string>("0", "Euclid"));
            galaxyDict.Add(new KeyValuePair<string, string>("1", "Hilbert"));
            galaxyDict.Add(new KeyValuePair<string, string>("2", "Calypso"));
            galaxyDict.Add(new KeyValuePair<string, string>("3", "Hesperius"));
            galaxyDict.Add(new KeyValuePair<string, string>("4", "Hyades"));
            galaxyDict.Add(new KeyValuePair<string, string>("5", "Ickjamatew"));
            galaxyDict.Add(new KeyValuePair<string, string>("6", "Bullangr"));
            galaxyDict.Add(new KeyValuePair<string, string>("7", "Kikolgallr"));
            galaxyDict.Add(new KeyValuePair<string, string>("8", "Eltiensleem"));
            galaxyDict.Add(new KeyValuePair<string, string>("9", "Eissentam"));
            galaxyDict.Add(new KeyValuePair<string, string>("10", "Elkupalos"));
            galaxyDict.Add(new KeyValuePair<string, string>("11", "Aptarkaba"));
            galaxyDict.Add(new KeyValuePair<string, string>("12", "Ontiniangp"));
            galaxyDict.Add(new KeyValuePair<string, string>("13", "Odiwagiri"));
            galaxyDict.Add(new KeyValuePair<string, string>("14", "Ogtialabi"));
            galaxyDict.Add(new KeyValuePair<string, string>("15", "Muhacksonto"));
            galaxyDict.Add(new KeyValuePair<string, string>("16", "Hitonskyer"));
            galaxyDict.Add(new KeyValuePair<string, string>("17", "Rerasmutul"));
            galaxyDict.Add(new KeyValuePair<string, string>("18", "Isdoraijung"));
            galaxyDict.Add(new KeyValuePair<string, string>("19", "Doctinawyra"));
            galaxyDict.Add(new KeyValuePair<string, string>("20", "Loychazinq"));
            galaxyDict.Add(new KeyValuePair<string, string>("21", "Zukasizawa"));
            galaxyDict.Add(new KeyValuePair<string, string>("22", "Ekwathore"));
            galaxyDict.Add(new KeyValuePair<string, string>("23", "Yeberhahne"));
            galaxyDict.Add(new KeyValuePair<string, string>("24", "Twerbetek"));
            galaxyDict.Add(new KeyValuePair<string, string>("25", "Sivarates"));
            galaxyDict.Add(new KeyValuePair<string, string>("26", "Eajerandal"));
            galaxyDict.Add(new KeyValuePair<string, string>("27", "Aldukesci"));
            galaxyDict.Add(new KeyValuePair<string, string>("28", "Wotyarogii"));
            galaxyDict.Add(new KeyValuePair<string, string>("29", "Sudzerbal"));
            galaxyDict.Add(new KeyValuePair<string, string>("30", "Maupenzhay"));
            galaxyDict.Add(new KeyValuePair<string, string>("31", "Sugueziume"));
            galaxyDict.Add(new KeyValuePair<string, string>("32", "Brogoweldian"));
            galaxyDict.Add(new KeyValuePair<string, string>("33", "Ehbogdenbu"));
            galaxyDict.Add(new KeyValuePair<string, string>("34", "Ijsenufryos"));
            galaxyDict.Add(new KeyValuePair<string, string>("35", "Nipikulha"));
            galaxyDict.Add(new KeyValuePair<string, string>("36", "Autsurabin"));
            galaxyDict.Add(new KeyValuePair<string, string>("37", "Lusontrygiamh"));
            galaxyDict.Add(new KeyValuePair<string, string>("38", "Rewmanawa"));
            galaxyDict.Add(new KeyValuePair<string, string>("39", "Ethiophodhe"));
            galaxyDict.Add(new KeyValuePair<string, string>("40", "Urastrykle"));
            galaxyDict.Add(new KeyValuePair<string, string>("41", "Xobeurindj"));
            galaxyDict.Add(new KeyValuePair<string, string>("42", "Oniijialdu"));
            galaxyDict.Add(new KeyValuePair<string, string>("43", "Wucetosucc"));
            galaxyDict.Add(new KeyValuePair<string, string>("44", "Ebyeloof"));
            galaxyDict.Add(new KeyValuePair<string, string>("45", "Odyavanta"));
            galaxyDict.Add(new KeyValuePair<string, string>("46", "Milekistri"));
            galaxyDict.Add(new KeyValuePair<string, string>("47", "Waferganh"));
            galaxyDict.Add(new KeyValuePair<string, string>("48", "Agnusopwit"));
            galaxyDict.Add(new KeyValuePair<string, string>("49", "Teyaypilny"));
            galaxyDict.Add(new KeyValuePair<string, string>("50", "Zalienkosm"));
            galaxyDict.Add(new KeyValuePair<string, string>("51", "Ladgudiraf"));
            galaxyDict.Add(new KeyValuePair<string, string>("52", "Mushonponte"));
            galaxyDict.Add(new KeyValuePair<string, string>("53", "Amsentisz"));
            galaxyDict.Add(new KeyValuePair<string, string>("54", "Fladiselm"));
            galaxyDict.Add(new KeyValuePair<string, string>("55", "Laanawemb"));
            galaxyDict.Add(new KeyValuePair<string, string>("56", "Ilkerloor"));
            galaxyDict.Add(new KeyValuePair<string, string>("57", "Davanossi"));
            galaxyDict.Add(new KeyValuePair<string, string>("58", "Ploehrliou"));
            galaxyDict.Add(new KeyValuePair<string, string>("59", "Corpinyaya"));
            galaxyDict.Add(new KeyValuePair<string, string>("60", "Leckandmeram"));
            galaxyDict.Add(new KeyValuePair<string, string>("61", "Quulngais"));
            galaxyDict.Add(new KeyValuePair<string, string>("62", "Nokokipsechl"));
            galaxyDict.Add(new KeyValuePair<string, string>("63", "Rinblodesa"));
            galaxyDict.Add(new KeyValuePair<string, string>("64", "Loydporpen"));
            galaxyDict.Add(new KeyValuePair<string, string>("65", "Ibtrevskip"));
            galaxyDict.Add(new KeyValuePair<string, string>("66", "Elkowaldb"));
            galaxyDict.Add(new KeyValuePair<string, string>("67", "Heholhofsko"));
            galaxyDict.Add(new KeyValuePair<string, string>("68", "Yebrilowisod"));
            galaxyDict.Add(new KeyValuePair<string, string>("69", "Husalvangewi"));
            galaxyDict.Add(new KeyValuePair<string, string>("70", "Ovna'uesed"));
            galaxyDict.Add(new KeyValuePair<string, string>("71", "Bahibusey"));
            galaxyDict.Add(new KeyValuePair<string, string>("72", "Nuybeliaure"));
            galaxyDict.Add(new KeyValuePair<string, string>("73", "Doshawchuc"));
            galaxyDict.Add(new KeyValuePair<string, string>("74", "Ruckinarkh"));
            galaxyDict.Add(new KeyValuePair<string, string>("75", "Thorettac"));
            galaxyDict.Add(new KeyValuePair<string, string>("76", "Nuponoparau"));
            galaxyDict.Add(new KeyValuePair<string, string>("77", "Moglaschil"));
            galaxyDict.Add(new KeyValuePair<string, string>("78", "Uiweupose"));
            galaxyDict.Add(new KeyValuePair<string, string>("79", "Nasmilete"));
            galaxyDict.Add(new KeyValuePair<string, string>("80", "Ekdaluskin"));
            galaxyDict.Add(new KeyValuePair<string, string>("81", "Hakapanasy"));
            galaxyDict.Add(new KeyValuePair<string, string>("82", "Dimonimba"));
            galaxyDict.Add(new KeyValuePair<string, string>("83", "Cajaccari"));
            galaxyDict.Add(new KeyValuePair<string, string>("84", "Olonerovo"));
            galaxyDict.Add(new KeyValuePair<string, string>("85", "Umlanswick"));
            galaxyDict.Add(new KeyValuePair<string, string>("86", "Henayliszm"));
            galaxyDict.Add(new KeyValuePair<string, string>("87", "Utzenmate"));
            galaxyDict.Add(new KeyValuePair<string, string>("88", "Umirpaiya"));
            galaxyDict.Add(new KeyValuePair<string, string>("89", "Paholiang"));
            galaxyDict.Add(new KeyValuePair<string, string>("90", "Iaereznika"));
            galaxyDict.Add(new KeyValuePair<string, string>("91", "Yudukagath"));
            galaxyDict.Add(new KeyValuePair<string, string>("92", "Boealalosnj"));
            galaxyDict.Add(new KeyValuePair<string, string>("93", "Yaevarcko"));
            galaxyDict.Add(new KeyValuePair<string, string>("94", "Coellosipp"));
            galaxyDict.Add(new KeyValuePair<string, string>("95", "Wayndohalou"));
            galaxyDict.Add(new KeyValuePair<string, string>("96", "Smoduraykl"));
            galaxyDict.Add(new KeyValuePair<string, string>("97", "Apmaneessu"));
            galaxyDict.Add(new KeyValuePair<string, string>("98", "Hicanpaav"));
            galaxyDict.Add(new KeyValuePair<string, string>("99", "Akvasanta"));
            galaxyDict.Add(new KeyValuePair<string, string>("100", "Tuychelisaor"));
            galaxyDict.Add(new KeyValuePair<string, string>("109", "Nudquathsenfe"));
            galaxyDict.Add(new KeyValuePair<string, string>("118", "Torweierf"));
            galaxyDict.Add(new KeyValuePair<string, string>("129", "Broomerrai"));
            galaxyDict.Add(new KeyValuePair<string, string>("138", "Emiekereks"));
            galaxyDict.Add(new KeyValuePair<string, string>("149", "Zavainlani"));
            galaxyDict.Add(new KeyValuePair<string, string>("158", "Rycempler"));
            galaxyDict.Add(new KeyValuePair<string, string>("169", "Ezdaranit"));
            galaxyDict.Add(new KeyValuePair<string, string>("178", "Wepaitvas"));
            galaxyDict.Add(new KeyValuePair<string, string>("189", "Cugnatachh"));
            galaxyDict.Add(new KeyValuePair<string, string>("198", "Horeroedsh"));
            galaxyDict.Add(new KeyValuePair<string, string>("209", "Digarlewena"));
            galaxyDict.Add(new KeyValuePair<string, string>("218", "Chmageaki"));
            galaxyDict.Add(new KeyValuePair<string, string>("229", "Raldwicarn"));
            galaxyDict.Add(new KeyValuePair<string, string>("238", "Yuwarugha"));
            galaxyDict.Add(new KeyValuePair<string, string>("249", "Nepitzaspru"));
            //galaxyDict.Add(new KeyValuePair<string, string>("", ""));

            galaxyDict.Add(new KeyValuePair<string, string>("140", "Kimycuristh"));
        }
        private void GalaxyLookup(TextBox source, string galaxy)
        {
            //lookup the galaxy and if not in galaxy dict, display the number
            try
            {
                source.Text = galaxyDict[galaxy];
            }
            catch
            {
                source.Text = (Convert.ToInt32(galaxy) + 1).ToString();
                //AppendLine(textBox17, "Galaxy Not Found, update needed.");
            }
        }
        private void OpenBackupFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Open the \backup dir in file explorer
            Process.Start(@".\backup");
        }
        private async void AppDataDefaultToolStripMenuItem_Click(object sender, EventArgs e)
        {
            nmsPath = Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "HelloGames"), "NMS");
            SetPath();

            ClearAll();            

            //ClearA();
            LoadCmbxA();
            ComboBox2_SelectionChangeCommitted(this, new EventArgs());

            await Task.Delay(300);

            //ClearB();
            LoadCmbxB();
            ComboBox4_SelectionChangeCommitted(this, new EventArgs());
        }
        private async void ManuallySelectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Manually select a save hg file path
            SelectSavePath();

            ClearAll();

            //ClearA();
            LoadCmbxA();
            ComboBox2_SelectionChangeCommitted(this, new EventArgs());

            await Task.Delay(300);

            //ClearB();
            LoadCmbxB();
            ComboBox4_SelectionChangeCommitted(this, new EventArgs());

        }
        private void SelectSavePath()
        {
            //Manually set save path method
            try
            {
                using (var fbd = new FolderBrowserDialog())
                {
                    DialogResult result = fbd.ShowDialog();

                    if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                    {
                        //Checks for hg files in the selected dir
                        string[] files = Directory.GetFiles(fbd.SelectedPath, "save*.hg");

                        if (files.Length != 0)
                        {
                            nmsPath = fbd.SelectedPath;
                            SetPath();
                            //AppendLine(textBox16, fbd.SelectedPath + "save.hg");

                            Read("nmsPath", savePath);
                            if (Directory.Exists(ReadKey))
                            {
                                MessageBox.Show("Path to save files set.", "Confirmation");
                            }
                            else
                            {
                                MessageBox.Show("Something went wrong!", "Alert");
                            }
                        }
                        else
                        {
                            MessageBox.Show("No Save files found! ", "Message");
                        }
                    }
                    else if (result == DialogResult.Cancel)
                    {
                        MessageBox.Show("Cancelled no path set!");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\r\n\r\n Save path problem!");
                return;
            }
        }
        private void BackupALLSaveFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Manually backup all save files in nmspath dir
            RunBackupAll(hgFileDir);
            MessageBox.Show("Save Backup Completed!", "Confirmation", MessageBoxButtons.OK);
        }
        private void CreateBackupDir()
        {
            //Checks for the json edit dir \json and creates if don't exist
            if (!Directory.Exists(@".\backup"))
            {
                Directory.CreateDirectory(@".\backup");
                Directory.CreateDirectory(@".\backup\json");
            }
            if (!Directory.Exists(@".\backup\json"))
            {
                Directory.CreateDirectory(@".\backup\json");
            }
        }
        public void BuildSaveFile()
        {
            //if save.nmsls doesn't exist, create it
            if (!File.Exists(savePath))
            {
                File.Create(savePath).Close();
                TextWriter tw = new StreamWriter(savePath);
                tw.WriteLine("nmsPath=" + nmsPath);
                tw.Close();
            }
            else if (File.Exists(savePath))
            {
                return;
            }
        }
        public void ReloadSave()
        {
            Read("nmsPath", savePath);
            nmsPath = ReadKey;

            //If save.nmsls gets corrupt, back to defaults on nmsPath
            if (Directory.Exists(nmsPath))
            {
                DirectoryInfo dinfo = new DirectoryInfo(nmsPath);
                if (dinfo.GetFiles("save*.hg", SearchOption.TopDirectoryOnly).Length <= 0)
                {
                    nmsPath = Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "HelloGames"), "NMS");
                }
            }
            else if (!Directory.Exists(nmsPath))
            {
                nmsPath = Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "HelloGames"), "NMS");
            }
        }
        private void Read(string key, string path)
        {
            ReadKey = "";

            try
            {
                if (!File.Exists(path))
                {
                    return;
                }
                List<string> filelist = File.ReadAllLines(path).ToList();
                foreach (string lst in filelist)
                {
                    if (lst.Contains(key))
                    {
                        //grabs the end of the line before key
                        Regex myRegexLS = new Regex(".*?=", RegexOptions.Multiline);
                        Match m1 = myRegexLS.Match(lst);
                        string line = m1.ToString();
                        line = line.Replace("=", "");

                        if (line == key)
                        {
                            string record = filelist.IndexOf(lst).ToString();

                            //grabs the end of the line after key
                            Regex myRegexRC = new Regex("=.*?$", RegexOptions.Multiline);
                            Match m2 = myRegexRC.Match(lst);
                            string line2 = m2.ToString();

                            //line2 = line2.Replace("=", "");
                            //string value = line2.Replace(" ", "");

                            string value = line2.Replace("=", "");
                            ReadKey = value;
                            //AppendLine(textBox4, "Read: " + key + "=" + value);
                        }
                    }
                }
            }
            catch
            {
                return;
            }
        }
        private void Write(string key, string newKey, string path)
        {
            try
            {
                if (!File.Exists(path))
                {
                    return;
                }
                List<string> filelist = File.ReadAllLines(path).ToList();
                foreach (string lst in filelist)
                {
                    if (lst.Contains(key))
                    {
                        //grabs the end of the line before key
                        Regex myRegexLS = new Regex(".*?=", RegexOptions.Multiline);
                        Match m1 = myRegexLS.Match(lst);
                        string line = m1.ToString();
                        line = line.Replace("=", "");

                        if (line == key)
                        {
                            //grabs the end of the line after key
                            Regex myRegexRC = new Regex("=.*?$", RegexOptions.Multiline);
                            Match m2 = myRegexRC.Match(lst);
                            string line2 = m2.ToString();

                            //line2 = line2.Replace("=", "");
                            //string value = line2.Replace(" ", "");

                            string value = line2.Replace("=", "");

                            //replaces the record at the selected filelist index
                            int index = filelist.IndexOf(lst);
                            string record = lst.Replace(value, newKey);
                            filelist[index] = record;
                            //AppendLine(textBox4, "Write: " + key + "=" + newKey);
                            File.WriteAllLines(path, filelist);
                            return;
                        }
                    }
                }
            }
            catch
            {
                return;
            }
        }        
    }
}
