using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
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
        bool IsIntel;
        private string make_h_file2(string filePath)
        {
            IsIntel = ByteOrderCheckBox.Checked;
            string[] sigcom;
            string main_msg = string.Empty;
            var dbc = Parser.ParseFromPath(filePath);
            main_msg += "/**\n" +
            " * @file d2cc_lib.h\n" +
            " * @brief Header file for the D2CC library.\n" +
            " * \n" +
            " * @date " + DateTime.Now.Date.ToString("yyyy-MM-dd") + "\n" +
            " * @author hakimmc\n" +
            " * @see https://www.linkedin.com/in/abdulhakim-calgin/\n" +
            " */\n\n";

            main_msg += "#ifndef LIB_\r\n#define LIB_\r\n\r\n#include <stdint.h>\r\n#include <stdbool.h>\n\n";

            var sortedmsgs = dbc.Messages.OrderBy(messages => messages.Name);

            main_msg += $"\n" +
                   $"/** \r\n * @def READ_ENABLE\r\n * @brief If you want to use Readparse function, uncomment it!\r\n */\r\n//#define READ_ENABLE" +
                   $"\n";

            foreach (var msg in sortedmsgs)
            {
                main_msg += $"\n" +
                    $"/** \r\n * @def {msg.Name}_ID\r\n * @brief CAN ID for {msg.Name} messages.\r\n */\r\n#define {msg.Name}_ID \t {msg.ID}" +
                    $"\n";

                main_msg += $"\n" +
                    $"/** \r\n * @def {msg.Name}_DLC\r\n * @brief CAN DLC for {msg.Name} messages.\r\n */\r\n#define {msg.Name}_DLC \t {msg.DLC}" +
                    $"\n";
            }

            foreach (var msg in sortedmsgs)
            {
                var sortedSignals = msg.Signals.OrderBy(sig => sig.StartBit);
                foreach (var sig in sortedSignals)
                {
                    int elementOfMap = string.Join("\n", sig.ValueTableMap).Split(',').Length;
                    int index_counter = 1;
                    int enum_counter = 0;
                    var sortedtable = sig.ValueTableMap.OrderBy(tab => tab.Key);

                    if (string.Join("\n", sig.ValueTableMap).Trim() != string.Empty)
                    {
                        main_msg += "\ntypedef enum{\n";
                        // Add Doxygen comment for the enum
                        main_msg += "/** \n";
                        main_msg += " * @enum " + sig.Name + "_enum\n";
                        main_msg += " * @brief Enum for representing signal values for the " + sig.Name + " signal.\n";
                        main_msg += " * \n";
                        main_msg += " * This enum maps each value in the signal's value table to a corresponding identifier.\n";
                        main_msg += " * It is used to interpret the raw data received from the CAN bus and provides human-readable names for each value.\n";
                        main_msg += " * \n";
                        main_msg += " * @note Ensure the signal values are aligned with the data type and expected range of the signal.\n";
                        main_msg += " */\n";
                    }

                    foreach (var item in sortedtable)
                    {
                        index_counter++;
                        string[] value = new string[2];
                        value[0] = item.ToString().TrimStart().TrimEnd().Substring(1, item.ToString().Length - 2).Split(',')[0];
                        value[1] = item.ToString().TrimStart().TrimEnd().Substring(1, item.ToString().Length - 2).Split(',')[1];

                        // Doxygen comment for the enum value
                        main_msg += "\t/** @brief " + value[1].TrimStart() + " */\n"; // Brief description of the enum value
                        main_msg += "\t" + fix_string(sig.Name.TrimEnd() + "_" + value[1].TrimStart());

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
                }
            }

            main_msg += "/** \n";
            main_msg += " * @brief factor, offset and value variable structer for CAN messages structure for message.\n";
            main_msg += " * \n";
            main_msg += " * This structure including factor, offset and value variables.\n";
            main_msg += " */\n";
            main_msg += "typedef struct {\r\n    float factor;\r\n    int offset;\r\n    float value;\r\n} Phys_Value_t;\n\n";

            main_msg += "typedef struct{\n";

            foreach (var msg in sortedmsgs)
            {
                // Doxygen comment for each message structure
                main_msg += "/** \n";
                main_msg += " * @brief CAN message structure for message " + msg.Name + ".\n";
                try
                {
                    if (msg.Comment != null)
                    {
                        main_msg += $" * {msg.Comment}\n";
                    }
                    else
                    {
                    }
                }
                catch { }
                
                main_msg += " */\n";
                main_msg += start_line_msg(msg.Name);

                //var sortedSignals = msg.Signals.OrderBy(sig => sig.StartBit); // REMOVED AT 26.05.2025 FOR ABOUT INTEL/MOTOROLA BYTE ENDIAN ERRORS
                var sortedSignals = msg.Signals.OrderBy(sig =>
                {
                    
                    if (IsIntel) // Intel
                    {
                        int byteIndex = sig.StartBit / 8;
                        int bitIndex = sig.StartBit % 8;
                        return (byteIndex * 8) + (7 - bitIndex);
                    }
                    else // Motorola
                    {
                        return sig.StartBit;
                    }
                });
                main_msg += "\tstruct{\n";

                // Union for signal values
                main_msg += "\t\tunion{\n";
                main_msg += "\t\t\tstruct{\n";
                foreach (var sig in sortedSignals)
                {
                    // Doxygen comment for each signal field
                    if (string.Join("\n", sig.ValueTableMap).Trim() != string.Empty)
                    {
                        main_msg += "\t\t\t\t/** \n";
                        main_msg += "\t\t\t\t * @brief Signal " + sig.Name + " enum type.\n";
                        if (sig.Comment != null)
                        {
                            sigcom = sig.Comment.Split('\n');
                            if (sig.Comment.Length > 0)
                            {
                                for (int i = 0; i < sigcom.Count(); i++)
                                {
                                    main_msg += $"\t\t\t\t * {sigcom[i]}\n";
                                }
                            }
                        }
                        main_msg += "\t\t\t\t */\n";
                        main_msg += "\t\t\t\t" + sig.Name + "_enum " + sig.Name + ":" + sig.Length + "; //" + sig.Length + " bit\n";
                    }
                    else
                    {
                        // Doxygen comment for different data types based on signal length
                        if (sig.Length <= 8)
                        {
                            main_msg += "\t\t\t\t/**\n";
                            main_msg += "\t\t\t\t * @brief " + sig.Name + " signal with 8-bit length.\n";
                            if (sig.Comment != null)
                            {
                                sigcom = sig.Comment.Split('\n');
                                if (sig.Comment.Length > 0)
                                {
                                    for (int i = 0; i < sigcom.Count(); i++)
                                    {
                                        main_msg += $"\t\t\t\t * {sigcom[i]}\n";
                                    }
                                }
                            }
                            main_msg += "\t\t\t\t */\n";
                            //main_msg += "\t\t\t\tuint8_t " + sig.Name + ":" + sig.Length + "; //" + sig.Length + " bit\n";
                            main_msg += "\t\t\t\tuint8_t " + sig.Name + (sig.Length != 8 ? (":" + sig.Length) : "") + "; //" + sig.Length + " bit\n";
                        }
                        else if (sig.Length <= 16)
                        {
                            main_msg += "\t\t\t\t/** \n";
                            main_msg += "\t\t\t\t * @brief " + sig.Name + " signal with 16-bit length.\n";
                            if (sig.Comment != null)
                            {
                                sigcom = sig.Comment.Split('\n');
                                if (sig.Comment.Length > 0)
                                {
                                    for (int i = 0; i < sigcom.Count(); i++)
                                    {
                                        main_msg += $"\t\t\t\t * {sigcom[i]}\n";
                                    }
                                }
                            }
                            main_msg += "\t\t\t\t */\n";
                            //main_msg += "\t\t\t\tuint16_t " + sig.Name + ":" + sig.Length + "; //" + sig.Length + " bit\n";
                            main_msg += "\t\t\t\tuint16_t " + sig.Name + (sig.Length != 16 ? (":" + sig.Length) : "") + "; //" + sig.Length + " bit\n";
                        }
                        else if (sig.Length <= 32)
                        {
                            main_msg += "\t\t\t\t/** \n";
                            main_msg += "\t\t\t\t * @brief " + sig.Name + " signal with 32-bit length.\n";
                            if (sig.Comment != null)
                            {
                                sigcom = sig.Comment.Split('\n');
                                if (sig.Comment.Length > 0)
                                {
                                    for (int i = 0; i < sigcom.Count(); i++)
                                    {
                                        main_msg += $"\t\t\t\t * {sigcom[i]}\n";
                                    }
                                }
                            }
                            main_msg += "\t\t\t\t */\n";
                            //main_msg += "\t\t\t\tuint32_t " + sig.Name + ":" + sig.Length + "; //" + sig.Length + " bit\n";
                            main_msg += "\t\t\t\tuint32_t " + sig.Name + (sig.Length != 32 ? (":" + sig.Length) : "") + "; //" + sig.Length + " bit\n";
                        }
                        else if (sig.Length <= 64)
                        {
                            main_msg += "\t\t\t\t/** \n";
                            main_msg += "\t\t\t\t * @brief " + sig.Name + " signal with 64-bit length.\n";
                            if (sig.Comment != null)
                            {
                                sigcom = sig.Comment.Split('\n');
                                if (sig.Comment.Length > 0)
                                {
                                    for (int i = 0; i < sigcom.Count(); i++)
                                    {
                                        main_msg += $"\t\t\t\t * {sigcom[i]}\n";
                                    }
                                }
                            }
                            main_msg += "\t\t\t\t */\n";
                            //main_msg += "\t\t\t\tuint64_t " + sig.Name + ":" + sig.Length + "; //" + sig.Length + " bit\n";
                            main_msg += "\t\t\t\tuint64_t " + sig.Name + (sig.Length != 64 ? (":" + sig.Length) : "") + "; //" + sig.Length + " bit\n";
                        }
                    }
                }
                main_msg += "\t\t\t}Signal;\n";
                main_msg += $"\t\t\tuint8_t Data[{msg.DLC}];\n";
                main_msg += "\t\t};\n";

                foreach (var sig in sortedSignals)
                {
                    if (sig.Factor != 1 || sig.Offset != 0)
                    {
                        // Doxygen comment for physical value structure
                        main_msg += "\t\t/** @brief Factor for scaling the raw signal value. */\n";
                        main_msg += "\t\tstruct{\n";
                        main_msg += "\t\t\tPhys_Value_t Phys_Value;\n";// + sig.Factor+"\n";
                        main_msg += "\t\t}" + sig.Name + ";\n";
                    }
                }

                // End Doxygen comment for message structure
                main_msg += "\t}" + msg.Name + ";\n" + end_line_msg(msg.Name);
            }

            // End Doxygen comment for the main structure
            main_msg += "}DbcStruct;\n";
            main_msg += "/*     USER CODE FUNCTION BLOCK START       */\n";

            
            UInt16 indx= 0;

            main_msg += $"\n/**\n";
            main_msg += " * @brief Initializes the DbcStruct for CAN message processing.\n";
            main_msg += " *\n";
            main_msg += " * This function should be called before using the DbcStruct to ensure that all\n";
            main_msg += " * message structures are properly initialized. It sets up necessary memory and \n";
            main_msg += " * configurations.\n";
            main_msg += " *\n";
            main_msg += " * @param[in] st Pointer to the DbcStruct instance that will be initialized.\n";
            main_msg += " */\n";
            main_msg += $"\nvoid D2cc_Lib_Init(DbcStruct *st); //Init Function (Must Be Run)\n";

            main_msg += $"\n#ifdef READ_ENABLE\n";
            main_msg += $"/**\n";
            main_msg += " * @brief Reads and parses CAN data.\n";
            main_msg += " *\n";
            main_msg += " * This function reads CAN data from the provided `rx_data` and parses it into the \n";
            main_msg += " * corresponding message and signal structures in the DbcStruct. It uses the CAN \n";
            main_msg += " * message ID to identify the message to parse.\n";
            main_msg += " *\n";
            main_msg += " * This function is only enabled if `READ_ENABLE` is defined.\n";
            main_msg += " *\n";
            main_msg += " * @param[in] rx_data Pointer to the received CAN data.\n";
            main_msg += " * @param[in] id The CAN message ID that is used to determine which message to parse.\n";
            main_msg += " * @param[in,out] st Pointer to the DbcStruct instance where the parsed data will be stored.\n";
            main_msg += " */\n";
            main_msg += $"\nvoid ReadParse(uint8_t* rx_data, uint32_t id, DbcStruct *st); //Can Read & Parse Function\n";
            main_msg += $"\n#endif\n";

            foreach (var msg in sortedmsgs)
            {
                main_msg += $"\n/**\n";
                main_msg += " * @brief Creates the signal table for the specified CAN message.\n";
                main_msg += " *\n";
                main_msg += " * This function initializes the signal table for the given CAN message. It \n";
                main_msg += " * organizes and prepares the signals in the message to be ready for processing \n";
                main_msg += " * or encoding.\n";
                main_msg += " *\n";
                main_msg += " * @param[in] dbc Pointer to the DbcStruct instance that contains the CAN message.\n";
                main_msg += " */\n";
                main_msg += $"\nvoid CreateTable_{msg.Name}(DbcStruct *dbc);\n";
            }

            main_msg += $"\n/**\n";
            main_msg += " * @brief Converts a physical value to its corresponding raw CAN value.\n";
            main_msg += " *\n";
            main_msg += " * This function takes a physical value and applies the scaling factor and offset \n";
            main_msg += " * specified in the `Phys_Value_t` structure to convert it to the raw CAN signal value.\n";
            main_msg += " * The raw value can then be transmitted in a CAN message.\n";
            main_msg += " *\n";
            main_msg += " * @param[in] phys_value The physical value to be converted.\n";
            main_msg += " * @param[in] phys_struct Pointer to the `Phys_Value_t` structure containing scaling and offset information.\n";
            main_msg += " *\n";
            main_msg += " * @return The corresponding raw CAN value.\n";
            main_msg += " */\n";
            main_msg += "\r\nuint32_t PHYS_TO_RAW(int phys_value, Phys_Value_t *phys_struct);\r\n";

            main_msg += $"\n/**\n";
            main_msg += " * @brief Converts a raw CAN value to its corresponding physical value.\n";
            main_msg += " *\n";
            main_msg += " * This function takes a raw CAN value and applies the scaling factor and offset \n";
            main_msg += " * specified in the `Phys_Value_t` structure to convert it to the physical signal value.\n";
            main_msg += " * The physical value represents the actual measured or calculated value of the signal.\n";
            main_msg += " *\n";
            main_msg += " * @param[in] raw_value The raw CAN signal value to be converted.\n";
            main_msg += " * @param[in] phys_struct Pointer to the `Phys_Value_t` structure containing scaling and offset information.\n";
            main_msg += " *\n";
            main_msg += " * @return The corresponding physical value.\n";
            main_msg += " */\n";
            main_msg += "\r\nuint32_t RAW_TO_PHYS(int raw_value, Phys_Value_t *phys_struct);\r\n";


            main_msg += "\n/*     USER CODE FUNCTION BLOCK STOP        */\n\n";
            main_msg += "#endif";
            return main_msg;
        }

        private string make_c_file2(string filePath)
{
    string main_msg = string.Empty;
    var dbc = Parser.ParseFromPath(filePath);


    main_msg += "/**\n" +
        " * @file d2cc_lib.c\n" +
        " * @brief Source file for the D2CC library.\n" +
        " * \n" +
        " * @date " + DateTime.Now.Date.ToString("yyyy-MM-dd") + "\n" +
        " * @author hakimmc\n" +
        " * @see https://www.linkedin.com/in/abdulhakim-calgin/\n" +
        " */\n\n";
            //main_msg += "/*\r\n *  d2cc_lib.c\r\n *\r\n *  Created on: " + DateTime.Now.Date.ToString().TrimEnd('0', ':') + "\r\n *  Author: hakimmc\r\n *\n *  https://www.linkedin.com/in/abdulhakim-calgin/\r\n *\n */\n\n";
            main_msg += "#include \"d2cc_lib.h\"\n\n";
    
    var sortedmsgs = dbc.Messages.OrderBy(messages => messages.Name);

    UInt16 indx = 0;

    main_msg += "\n/**\r\n * @brief Initializes the DbcStruct with values from the parsed DBC file.\r\n"
                + " *\r\n"
                + " * This function initializes the necessary fields in the DbcStruct based on the provided DBC messages.\r\n"
                + " * The signals' physical factors and offsets are set accordingly.\r\n"
                + " *\r\n"
                + " * @param[in] dbc Pointer to the DbcStruct instance to be initialized.\r\n"
                + " */\n";
    main_msg += "\nvoid D2cc_Lib_Init(DbcStruct *dbc){\n";

    foreach (var msg in sortedmsgs)
    {
        var sortedSignals = msg.Signals.OrderBy(sig => sig.StartBit);
        foreach (var sig in sortedSignals)
        {
            if (sig.Factor != 1 || sig.Offset != 0)
            {
                main_msg += "\tdbc->" + msg.Name+ "." + sig.Name + ".Phys_Value.factor\t=\t" + sig.Factor+";\n";
                main_msg += "\tdbc->" + msg.Name + "." + sig.Name + ".Phys_Value.offset\t=\t" + sig.Offset+";\n";
            }
        }
    }
    main_msg += "}\n\n";

    indx = 0;

    main_msg += "\n/**\r\n * @brief Reads and parses CAN data into the DbcStruct.\r\n"
                + " *\r\n"
                + " * This function is used to read CAN data and store it in the DbcStruct. It parses the data and converts\r\n"
                + " * physical values to raw CAN signal values based on the scaling factors and offsets defined in the DbcStruct.\r\n"
                + " *\r\n"
                + " * @param[in] rx_data Pointer to the received CAN data to be parsed.\r\n"
                + " * @param[in] id CAN message ID for identifying which message to parse.\r\n"
                + " * @param[in,out] dbc Pointer to the DbcStruct where parsed data will be stored.\r\n"
                + " */\n";
    main_msg += $"\n#ifdef READ_ENABLE\nvoid ReadParse(uint8_t* rx_data, uint32_t id, DbcStruct *dbc)";

    main_msg += "{\r\n    switch (id) {\n\n";
    foreach (var msg in sortedmsgs)
    {
        var sortedSignals = msg.Signals.OrderBy(sig => sig.StartBit);
        //main_msg += "\tcase 0x" + msg.ID.ToString("X") + ":\n";
        main_msg += "\tcase " + msg.Name+ "_ID :\n";
        main_msg += "\t\tfor(int i=0;i<" + msg.DLC + ";i++){\n" +
        "\t\t\tdbc->" + msg.Name + ".Data[i] = rx_data[i];\n\t\t}\n";
        foreach (var sig in sortedSignals)
        {
            if (sig.Factor != 1 || sig.Offset != 0)
            {
                main_msg += "\t\tdbc->" + msg.Name + "." +sig.Name+ ".Phys_Value.value\t=\t(dbc->"+ msg.Name + ".Signal." + sig.Name+" * "+sig.Factor+") + "+sig.Offset+";\n";
            }
        }
        main_msg += "\t\tbreak;\n";
    }
    main_msg += "\t}\n}\n#endif";

    main_msg += "\n/**\r\n * @brief Creates the signal table for a specified CAN message in the DbcStruct.\r\n"
                + " *\r\n"
                + " * This function initializes the signal table for a specific message within the DbcStruct. Each signal is \r\n"
                + " * set to 0 initially, preparing it for further use.\r\n"
                + " *\r\n"
                + " * @param[in] dbc Pointer to the DbcStruct containing the CAN message and signals.\r\n"
                + " */\n";
    foreach (var msg in sortedmsgs)
    {
        main_msg += $"\nvoid CreateTable_{msg.Name}(DbcStruct *dbc)\n";
        main_msg += "{\n";
        var sortedSignals = msg.Signals.OrderBy(sig => sig.StartBit);
        foreach (var sig in sortedSignals)
        {
            if((sig.Factor != 1) || (sig.Offset != 0))
            {
                main_msg += "\tdbc->" + msg.Name + ".Signal." + sig.Name + " = PHYS_TO_RAW(0,&dbc->"+ msg.Name+"."+sig.Name+".Phys_Value);\n";
            }
            else
            {
                main_msg += "\tdbc->" + msg.Name + ".Signal." + sig.Name + " = 0;\n";
            }
        }
        main_msg += "\n}";
    }

    main_msg += "\n/**\r\n * @brief Converts a physical value to a raw CAN signal value.\r\n"
                + " *\r\n"
                + " * This function converts a physical value (e.g., temperature) into a raw CAN signal value based on the \r\n"
                + " * scaling factor and offset stored in the Phys_Value_t structure.\r\n"
                + " *\r\n"
                + " * @param[in] phys_value The physical value to convert.\r\n"
                + " * @param[in] phys_struct Pointer to the Phys_Value_t structure containing scaling factor and offset.\r\n"
                + " *\r\n"
                + " * @return The corresponding raw CAN signal value.\r\n"
                + " */\n";
    main_msg += "\nuint32_t PHYS_TO_RAW(int phys_value, Phys_Value_t *phys_struct) \r\n{\r\n    return ((phys_value - phys_struct->offset) / phys_struct->factor);\r\n}\n";

    main_msg += "\n/**\r\n * @brief Converts a raw CAN signal value to a physical value.\r\n"
                + " *\r\n"
                + " * This function converts a raw CAN signal value to its corresponding physical value based on the \r\n"
                + " * scaling factor and offset stored in the Phys_Value_t structure.\r\n"
                + " *\r\n"
                + " * @param[in] raw_value The raw CAN signal value to convert.\r\n"
                + " * @param[in] phys_struct Pointer to the Phys_Value_t structure containing scaling factor and offset.\r\n"
                + " *\r\n"
                + " * @return The corresponding physical value.\r\n"
                + " */\n";
    main_msg += "\nuint32_t RAW_TO_PHYS(int raw_value, Phys_Value_t *phys_struct) \r\n{\r\n    return ((raw_value * phys_struct->factor) + phys_struct->offset);\r\n}";

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

        public string fix_string(string word)
        {
            return word.Replace("@", "").Replace("/", "_").Replace(".", "_").Replace("-", "_").Replace("?", "").Replace(" ","_");
        }
    }
}
