using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using DbcParserLib;
using DbcParserLib.Model;
using DbcParserLib.Observers;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace D2CC
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        string h_saved_name;
        int alg_index = 0;
        bool state = false;
        private void AddDbcFile_Button_Click(object sender, EventArgs e)
        {
            try
            {
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    fileaddress.Text = openFileDialog1.FileName;
                    filename.Text = openFileDialog1.SafeFileName;
                    openFileDialog1.InitialDirectory = @"" + fileaddress.Text;
                    button1.Enabled = true;
                    if (File.Exists(fileaddress.Text))
                    {
                        //LoadDbc(fileaddress.Text);
                        //MessageBox.Show(make_c_file(fileaddress.Text));
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        public string start_line_msg(string text)
        {
            return "/* " + text + " Line Start */\n";
        }
        public string end_line_msg(string text)
        {
            return "\n/* " + text + " Line End */\n\n";
        }

        private string make_h_file(string filePath)
        {
            string main_msg = string.Empty;
            var dbc = Parser.ParseFromPath(filePath);
            main_msg += "/*\r\n * d2cc_lib.h\r\n *\r\n *  Created on: " + DateTime.Now.Date.ToString().TrimEnd('0', ':') + "\r\n *      Author: hakimmc\r\n */\n\n";
            main_msg += "#ifndef LIB_\r\n#define LIB_\r\n\r\n#include <stdint.h>\r\n#include <stdbool.h>\n\n";
            foreach (var msg in dbc.Messages)
            {
                var sortedSignals = msg.Signals.OrderBy(sig => sig.StartBit);
                main_msg += start_line_msg(msg.Name) + "\nstruct{\n";
                main_msg += "\tuint32_t ID;\n";// = 0x" + msg.ID.ToString("X") + ";\n";
                main_msg += "\tuint8_t DLC;\n";// = " + msg.DLC + ";\n";
                /*main_msg += "\tenum Value{";
                int value_indx = 0;
                foreach (var sig in sortedSignals)
                {
                    string valuemap = string.Join("\n", sig.ValueTableMap);
                    MessageBox.Show("\n" + sig.ValueTableMap[1].ToString() +":"+sig.ValueTableMap[1][0].ToString());
                }*/
                main_msg += "\tunion{\n";
                main_msg += "\t\tuint8_t Message[" + msg.DLC + "];\n";
                main_msg += "\t\tstruct{\n";
                /*Algorithm 0*/
                if (alg_index == 0)
                {
                    foreach (var sig in sortedSignals)
                    {
 
                        main_msg += "\t\t\tuint64_t " + sig.Name + ":" + sig.Length + ";\n";
                        //dtSignals.Rows.Add("0x" + sig.ID.ToString("X"), sig.Name, sig.StartBit, sig.Length, sig.ByteOrder, sig.ValueType, sig.InitialValue, sig.Factor, sig.Offset, sig.Minimum, sig.Maximum, sig.Unit, valueTableString, sig.Comment);

                    }
                    main_msg += "\t\t}Bits;\n";
                    main_msg += "\t}Data;\n";
                    main_msg += "}" + msg.Name + ";\n" + end_line_msg(msg.Name);
                }
                else
                {
                    foreach (var sig in sortedSignals)
                    {
                        
                        main_msg += "\t\t\tuint64_t " + sig.Name + ":" + sig.Length + ";\n";
                        //dtSignals.Rows.Add("0x" + sig.ID.ToString("X"), sig.Name, sig.StartBit, sig.Length, sig.ByteOrder, sig.ValueType, sig.InitialValue, sig.Factor, sig.Offset, sig.Minimum, sig.Maximum, sig.Unit, valueTableString, sig.Comment);

                    }
                    main_msg += "\t\t}Bits;\n";
                    main_msg += "\t}Data;\n";
                    main_msg += "}" + msg.Name + ";\n" + end_line_msg(msg.Name);
                }
            }

            main_msg += "\nvoid D2cc_Lib_Init();//Init Function (Must Be Run)\n#endif";
            return main_msg;
        }

        private string make_c_file(string filePath)
        {
            string main_msg = string.Empty;
            var dbc = Parser.ParseFromPath(filePath);
            main_msg += "#include \"d2cc_lib.h\"\n\n";
            main_msg += "void D2cc_Lib_Init(){\n";
            foreach (var msg in dbc.Messages)
            {
                main_msg += "\t" + msg.Name + ".ID = 0x" + msg.ID.ToString("X") + ";\n";
                main_msg += "\t" + msg.Name + ".DLC = " + msg.DLC.ToString() + ";\n";
            }
            main_msg += "}\n\n";

            return main_msg;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            button1.Enabled = false;
            button2.Enabled = false;
            openFileDialog1.InitialDirectory = "@C";
            openFileDialog1.Title = "Browse dbc Files";
            openFileDialog1.Filter = "dbc files (*.dbc)|*.dbc";
            //MessageBox.Show(find_bitcount_to_maxvalue(8).ToString("X"));

        }

        private void button1_Click(object sender, EventArgs e)
        {
            SaveFileDialog save = new SaveFileDialog();
            save.RestoreDirectory = true;
            save.FileName = "d2cc_lib.h";
            save.OverwritePrompt = false;
            save.CreatePrompt = true;
            save.Title = "C Header File";
            save.DefaultExt = "h";
            save.Filter = "C Header File (*.h)|*.h";
            if (save.ShowDialog() == DialogResult.OK)
            {
                StreamWriter Kayit = new StreamWriter(save.FileName);
                Kayit.WriteLine(make_h_file2(fileaddress.Text));
                Kayit.Close();
                MessageBox.Show("Header File (.h) saved on " + save.FileName+".", "Succesfull!", MessageBoxButtons.OK, MessageBoxIcon.Information); ;
            }
            h_saved_name = save.FileName;
            button2.Enabled = true;
        }
        string save_file_name;
        private void button2_Click(object sender, EventArgs e)
        {
            SaveFileDialog save = new SaveFileDialog();
            save.RestoreDirectory = true;
            save.OverwritePrompt = false;
            save.CreatePrompt = true;
            save.Title = "C Source File";
            save.DefaultExt = "c";
            save.Filter = "C Source File (*.c)|*.c";
            save.FileName = "d2cc_lib.c";
            if (save.ShowDialog() == DialogResult.OK)
            {
                StreamWriter Kayit = new StreamWriter(save.FileName);
                save_file_name = save.FileName;
                Kayit.WriteLine(make_c_file2(fileaddress.Text));
                Kayit.Close();
                MessageBox.Show("Source File (.c) saved on " + save.FileName + ".", "Succesfull!", MessageBoxButtons.OK, MessageBoxIcon.Information); ;
            }
        }
        private string make_h_file2(string filePath)
        {
            string main_msg = string.Empty;
            var dbc = Parser.ParseFromPath(filePath);
            main_msg += "/*\r\n *  d2cc_lib.h\r\n *\r\n *  Created on: " + DateTime.Now.Date.ToString().TrimEnd('0', ':') + "\r\n *  Author: hakimmc\r\n *\n *  https://www.linkedin.com/in/abdulhakim-calgin/\r\n *\n */\n\n";
            main_msg += "#ifndef LIB_\r\n#define LIB_\r\n\r\n#include <stdint.h>\r\n#include <stdbool.h>\n\n";
            var sortedmsgs = dbc.Messages.OrderBy(messages => messages.Name);
            foreach (var msg in sortedmsgs)
            {
                main_msg += start_line_msg(msg.Name);
                var sortedSignals = msg.Signals.OrderBy(sig => sig.StartBit);


                foreach (var sig in sortedSignals)
                {
                    int elementOfMap = string.Join("\n", sig.ValueTableMap).Split(',').Length;
                    int index_counter = 1;
                    int enum_counter = 0;
                    var sortedtable = sig.ValueTableMap.OrderBy(tab => tab.Key);
                    if(string.Join("\n", sig.ValueTableMap).Trim() != string.Empty)
                    {
                        main_msg += "\ntypedef enum{\n";
                    }
                    foreach (var item in sortedtable)
                    {
                        index_counter++;
                        string[] value = new string[2];
                        value[0] = item.ToString().TrimStart().TrimEnd().Substring(1, item.ToString().Length - 2).Split(',')[0];
                        value[1] = item.ToString().TrimStart().TrimEnd().Substring(1, item.ToString().Length - 2).Split(',')[1];
                        main_msg += "\t" + sig.Name.TrimEnd() + "_" + value[1].TrimStart();
                        if (enum_counter != Convert.ToInt32(value[0]))
                        {
                            main_msg += " = 0x" + Convert.ToInt32(value[0].TrimStart()).ToString("X");
                            enum_counter = Convert.ToInt32(value[0]);
                        }
                        enum_counter++;
                        if (index_counter != elementOfMap)
                        {
                            main_msg += ",";
                        }
                    }
                    if (string.Join("\n", sig.ValueTableMap).Trim() != string.Empty)
                    {
                        main_msg += "\n}" + sig.Name + "_enum;\n\n";
                    }
                    //MessageBox.Show(main_msg);
                }
                main_msg += "struct " + msg.Name + "{\n";
                main_msg += "\tuint32_t ID;\n";// = 0x" + msg.ID.ToString("X") + ";\n";
                main_msg += "\tuint8_t DLC;\n";// = " + msg.DLC + ";\n";
                main_msg += "\tstruct{\n";
                foreach (var sig in sortedSignals)
                {
                    if (string.Join("\n", sig.ValueTableMap).Trim() != string.Empty)
                    {
                        main_msg += "\t\t"+sig.Name+"_enum " + sig.Name + "; //" + sig.Length + " bit\n";
                    }
                    else
                    {
                        if (sig.Length <= 8)
                        {
                            main_msg += "\t\tuint8_t " + sig.Name + "; //" + sig.Length + " bit\n";
                        }
                        else if (sig.Length <= 16)
                        {
                            main_msg += "\t\tuint16_t " + sig.Name + "; //" + sig.Length + " bit\n";
                        }
                        else if (sig.Length <= 32)
                        {
                            main_msg += "\t\tuint32_t " + sig.Name + "; //" + sig.Length + " bit\n";
                        }
                        else if (sig.Length <= 64)
                        {
                            main_msg += "\t\tuint64_t " + sig.Name + "; //" + sig.Length + " bit\n";
                        }
                    }

                }
                main_msg += "\t}Signal;\n";

                foreach (var sig in sortedSignals)
                {
                    if(sig.Factor != 1 || sig.Offset != 0)
                    {
                        main_msg += "\tstruct{\n";
                        main_msg += "\t\tstruct{\n";
                        main_msg += "\t\t\tfloat factor;\n";// + sig.Factor+"\n";
                        main_msg += "\t\t\tint offset;\n";// + sig.Offset + "\n";
                        main_msg += "\t\t\tfloat value;\n";
                        main_msg += "\t\t}" + sig.Name+";\n";
                        main_msg += "\t}Phys_Value;\n";
                    }
                }

                main_msg += "};\n" + end_line_msg(msg.Name);
            }
            main_msg += "/*     USER CODE FUNCTION BLOCK START       */\n";

            
            UInt16 indx= 0;

            foreach (var msg in sortedmsgs)
            {
                main_msg += $"\nvoid D2cc_Lib_Init(struct {msg.Name} *st{++indx}";
                break;
            }

            foreach (var msg in sortedmsgs)
            {
                indx++;
                main_msg += $",\n\t\tstruct {msg.Name} *st{indx}";
            }
            main_msg += "); //Init Function (Must Be Run)\n";



            indx = 0;
            foreach (var msg in sortedmsgs)
            {
                main_msg += $"\nvoid ReadParse(uint8_t* rx_data, uint32_t id, struct {msg.Name} *st{++indx}";
                break;
            }
            
            foreach (var msg in sortedmsgs)
            {
                main_msg += $",\n\t\tstruct {msg.Name} *st{++indx}";
            }

            main_msg += "); //Can Read & Parse Function\n";
            main_msg += "\n/*     USER CODE FUNCTION BLOCK STOP        */\n\n";
            main_msg += "#endif";
            return main_msg;
        }

        private string make_c_file2(string filePath)
        {
            string main_msg = string.Empty;
            var dbc = Parser.ParseFromPath(filePath);
            main_msg += "/*\r\n *  d2cc_lib.c\r\n *\r\n *  Created on: " + DateTime.Now.Date.ToString().TrimEnd('0', ':') + "\r\n *  Author: hakimmc\r\n *\n *  https://www.linkedin.com/in/abdulhakim-calgin/\r\n *\n */\n\n";
            main_msg += "#include \"d2cc_lib.h\"\n\n";
            var sortedmsgs = dbc.Messages.OrderBy(messages => messages.Name);

            UInt16 indx = 0;

            foreach (var msg in sortedmsgs)
            {
                main_msg += $"\nvoid D2cc_Lib_Init(struct {msg.Name} *st{++indx}";
                break;
            }

            foreach (var msg in sortedmsgs)
            {
                indx++;
                main_msg += $", struct {msg.Name} *st{indx}";
            }
            main_msg += "){";

            foreach (var msg in sortedmsgs)
            {
                var sortedSignals = msg.Signals.OrderBy(sig => sig.StartBit);
                main_msg += "\t" + msg.Name + "->ID\t=\t0x" + msg.ID.ToString("X") + ";\n";
                main_msg += "\t" + msg.Name + "->DLC\t=\t" + msg.DLC.ToString() + ";\n";
                foreach (var sig in sortedSignals)
                {
                    if (sig.Factor != 1 || sig.Offset != 0)
                    {
                        main_msg += "\t"+msg.Name+ ".Phys_Value."+sig.Name+ ".factor\t\t=\t" + sig.Factor+";\n";
                        main_msg += "\t" + msg.Name + ".Phys_Value." + sig.Name + ".offset\t\t=\t" + sig.Offset+";\n";
                    }
                }
            }
            main_msg += "}\n\n";

            indx = 0;
            foreach (var msg in sortedmsgs)
            {
                main_msg += $"\nvoid ReadParse(uint8_t* rx_data, uint32_t id, struct {msg.Name} *st{++indx}";
                break;
            }

            foreach (var msg in sortedmsgs)
            {
                main_msg += $",\n\t\tstruct {msg.Name} *st{++indx}";
            }

            main_msg += "){\r\n    switch (id) {\n\n";
            foreach (var msg in sortedmsgs)
            {
                var sortedSignals = msg.Signals.OrderBy(sig => sig.StartBit);
                main_msg += "\tcase 0x" + msg.ID.ToString("X") + ":\n";
                foreach (var sig in sortedSignals)
                {
                    main_msg += "\t\t"+msg.Name + ".Signal." + sig.Name    +"\t\t=\t";
                    int hex_bitwise = find_bitcount_to_maxvalue(sig.Length);
                    if (sig.Length < 8)
                    {
                        //ANDLENMESI GEREKEN TEK BYTELAR
                        if (hex_bitwise < 0xFF)
                        {
                            main_msg += "(" + get_rxdataindex(sig.Length + sig.StartBit);
                            main_msg += " & (0x" + hex_bitwise.ToString("X");
                            main_msg += "<<" + (sig.StartBit - rx_data_index * 8) + "))>>" + (sig.StartBit - rx_data_index * 8) + ";\n";
                        }
                    }
                    else
                    {
                        // ANDLENMESI GEREKMEYEN TEK BYTELAR
                        if (sig.Length == 8)
                        {
                            main_msg += "(" + get_rxdataindex(sig.Length + sig.StartBit);
                            main_msg += ");\n";
                        }

                        // ORLANMASI VE ANDLENMESI GEREKEN BYTELAR
                        else
                        {

                            //STARTBITI 0DAN BASLAMAYAN BYTELAR
                            if(sig.StartBit%8 != 0)
                            {
                                //YAZILACAK
                                //MessageBox.Show(sig.Name);
                            }
                            else
                            {

                            int msg_len = sig.Length;
                            int wise_count = sig.Length - sig.Length%8;
                            hex_bitwise = sig.Length%8 == 0 ? 0xFF : find_bitcount_to_maxvalue(sig.Length%8);
                                
                                while (true)
                                {
                                    hex_bitwise = find_bitcount_to_maxvalue(msg_len % 8==0?8: msg_len % 8); //+(sig.StartBit%8));
                                    main_msg += "(((" + get_rxdataindex(msg_len) + ") & (0x" + hex_bitwise.ToString("X") + "))<<" + rx_data_index*8+")";
                                    //wise_count -= 8;
                                    msg_len = (rx_data_index) * 8;
                                    //MessageBox.Show(wise_count + "__" + hex_bitwise);
                                    if (wise_count < 0)
                                    {
                                        break;
                                    }
                                    if (msg_len > 0)
                                    {
                                        main_msg += " | ";
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                            }
                            main_msg += ";\n";
                        }
                    }
                }
                foreach (var sig in sortedSignals)
                {
                    if (sig.Factor != 1 || sig.Offset != 0)
                    {
                        main_msg += "\t\t" + msg.Name + ".Phys_Value.value\t\t=\t(" + msg.Name+".Signal."+sig.Name+" * "+sig.Factor+") + "+sig.Offset+";\n";
                    }
                }
                main_msg += "\t\tbreak;\n";
            }
            main_msg += "\t}\n}";

            return main_msg;
        }

        private int find_bitcount_to_maxvalue(int bitcount)
        {
            int toplam_value=0;
            for(int i = 0; i < bitcount; i++)
            {
                toplam_value += 1<<i;
            }
            return toplam_value;
        }
        int rx_data_index = 0;

        private string get_rxdataindex(int lengthbit)
        {
            if (0 < lengthbit && lengthbit < 9) { rx_data_index = 0; return "rx_data[0]"; }
            if (8 <lengthbit && lengthbit  < 17) { rx_data_index = 1; return "rx_data[1]"; }
            if (16 < lengthbit && lengthbit < 25) { rx_data_index = 2; return "rx_data[2]"; }
            if (24 < lengthbit && lengthbit < 33) { rx_data_index = 3; return "rx_data[3]"; }
            if (32 < lengthbit && lengthbit < 41) { rx_data_index = 4; return "rx_data[4]"; }
            if (40 < lengthbit && lengthbit < 49) { rx_data_index = 5; return "rx_data[5]"; }
            if (48 < lengthbit && lengthbit < 57) { rx_data_index = 6; return "rx_data[6]"; }
            if (56 < lengthbit && lengthbit < 65) { rx_data_index = 7; return "rx_data[7]"; }
            return null;
        }
    }
}
