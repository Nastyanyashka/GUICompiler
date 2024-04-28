using Microsoft.Win32;
using System.IO;
using System.IO.Pipes;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace GUICompiler
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string currentFile = string.Empty;
        bool textChanged = false;
        public MainWindow()
        {

            InitializeComponent();
            this.Closing += MainWindow_Closing;
        }

        internal bool AskSave()
        {
            MessageBoxResult result = MessageBox.Show("Вы хотите сохранить изменения?", "", MessageBoxButton.YesNoCancel);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    if (currentFile == string.Empty)
                    {
                        try
                        {
                            SaveAsFile();
                        }
                        catch (Exception ex)
                        {
                            return false;
                        }
                    }
                    else { SaveFile(); }
                    break;
                case MessageBoxResult.No:
                    break;
                case MessageBoxResult.Cancel:
                    return false;
                default: break;

            }
            return true;
        }
        private void MainWindow_Closing(object? sender, System.ComponentModel.CancelEventArgs e)
        {
            if (textChanged == true)
            {
                if (AskSave() == false) { e.Cancel = true; }
            }
        }

        internal string CurrentFile { get { return currentFile; } set { currentFile = value; } }
        internal void CreateFile()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Text Files (*.txt)|*.txt| All files(*.*) | *.* ";
            if (saveFileDialog.ShowDialog() == true)
            {
                //string filename = saveFileDialog.FileName;

                //System.IO.File.Create(filename).Close();

                currentFile = saveFileDialog.FileName;
                File.Create(saveFileDialog.FileName).Close();

                MessageBox.Show("Файл создан");
            }
        }
        internal void SaveAsFile()
        {

            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "Text Files (*.txt)|*.txt|All files (*.*)|*.*";
            if (dlg.ShowDialog() == true)
            {
                using (FileStream fileStream = new FileStream(dlg.FileName, FileMode.Create))
                {
                    TextRange range = new TextRange(textEditor.Document.ContentStart, textEditor.Document.ContentEnd);

                    range.Save(fileStream, DataFormats.Text);
                }
            }
            else { throw new Exception(); }
        }
        internal void SaveFile()
        {
            using (FileStream fileStream = new FileStream(currentFile, FileMode.Create))
            {
                TextRange range = new TextRange(textEditor.Document.ContentStart, textEditor.Document.ContentEnd);
                range.Save(fileStream, DataFormats.Text);
            }
            //File.WriteAllText(currentFile,textBox.Text);
        }
        internal void OpenFile()
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Text Files (*.txt)|*.txt|All files (*.*)|*.*";
            if (dlg.ShowDialog() == true)
            {
                using (FileStream fileStream = new FileStream(dlg.FileName, FileMode.Open))
                {
                    currentFile = dlg.FileName;
                    TextRange range = new TextRange(textEditor.Document.ContentStart, textEditor.Document.ContentEnd);
                    range.Load(fileStream, DataFormats.Text);
                }
            }
        }


        private void Start_Click(object sender, RoutedEventArgs e)
        {


        }

        private void NewCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void NewCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (currentFile != string.Empty && textChanged == true)
            {
                if (AskSave() == false) { return; }
            }
            CreateFile();
            if (currentFile == string.Empty)
            {
                return;
            }
            TextRange range = new TextRange(textEditor.Document.ContentStart, textEditor.Document.ContentEnd);
            using (FileStream fs = new FileStream(currentFile, FileMode.Open))
            {
                range.Load(fs, DataFormats.Text);
            }
        }

        private void OpenCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void OpenCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (currentFile != string.Empty && textChanged == true)
            {
                if (AskSave() == false) { return; }
            }
            OpenFile();
        }

        private void SaveCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void SaveCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (currentFile == string.Empty)
            {
                try
                {
                    SaveAsFile();
                }
                catch (Exception ex) { }
            }
            else { SaveFile(); }
        }

        private void SaveAsCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void SaveAsCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            SaveAsFile();
        }

        private void CloseCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {

            e.CanExecute = true;
        }

        private void CloseCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            App.Current.Shutdown();
        }

        private void DeleteCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {

            e.CanExecute = (textEditor != null) && (textEditor.Selection.IsEmpty == false);
        }

        private void DeleteCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            textEditor.Selection.Text = string.Empty;
        }

        private void Help_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Справка-руководство пользователя\r\n" +
                "Создание документа\r\nДля создания нового документа на вкладке Файл или на панели элементов выберите пункт Создать \r\n\r\n" +
                "Открытие документа\r\n Для открытия документа на вкладке Файл или на панели элементов выберите пункт Открыть, в появившемся диалоговом окне выберите файл, который хотите открыть\r\n\r\nСохранение текущих изменений в документе\r\nДля сохранения текущих изменений в документе на вкладке Файл или на панели элементов выберите пункт Сохранить\r\n\r\n" +
                "Функция Сохранить как \r\nДля сохранения текущих изменений в документе с возможностью выбора названия файла, его размещения и формата, на вкладке Файл или на панели элементов выберите пункт Сохранить как. \r\n\r\nОтмена изменений \r\nЧтобы отменить последнее изменение файла на вкладке Правка или на панели элементов выберите пункт Отменить \r\n\r\n" +
                "Повтор последнего изменения\r\nЧтобы повторить последнее изменение файла на вкладке Правка или на панели элементов выберите пункт Повторить\r\n\r\nКопировать текстовый фрагмент \r\nВыделите нужный вам текстовый фрагмент и на вкладке Правка или на панели элементов выберите пункт Копировать, выделенный текст будет помещен в буфер обмена\r\n\r\n" +
                "Вырезать текстовый фрагмент\r\nВыделите нужный вам текстовый фрагмент и на вкладке Правка или панели элементов выберите пункт Вырезать, выделенный текст будет стерт их документа с одновременным копированием\r\n\r\n" +
                "Вставить текстовый фрагмент \r\nПереместите курсор на место вставки текста, на вкладке Правка или панели элементов выберите пункт Вставить, текст из буфера обмена будет вставлен после курсора\r\n\r\nВызов справки - руководства пользователя\r\nДля вызова справки-руководства пользователя на вкладке Справка или на панели элементов выберите пункт Вызов справки\r\n\r\n" +
                "Вызов информации о программе\r\nДля вызова информации о программе на вкладке Справка или на панели элементов выберите пункт О программе\r\n\r\nВыход из программы\r\nДля выхода из программы на вкладке Файл или на панели элементов выберите пункт Выход\r\n\r\nФункция Удалить\r\nВыделите нужный вам текстовый фрагмент и на вкладке Правка выберите пункт Удалить, выделенный текст будет стерт их документа без копирования\r\n\r\n" +
                "Функция Выделить все\r\nНа вкладке Правка выберите пункт Выделить все, на весь текст документа будет применено выделение\r\n", "Справка");
        }

        private void AboutProgramm_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("FZCE – программа, организующая базовые возможности по редактированию текста, сохранению и открытию текстовых файлов." +
                "\r\nВерсия: 0.1-Alpha\r\nЛицензия: MIT LICENSE\r\nЛокаль: RU-ru\r\nПользовательский интерфейс: C#&WF\r\nДата: 15.02.24",
                "О программе");
        }

        private void textEditor_TextChanged(object sender, TextChangedEventArgs e)
        {
            //if (!textChanged) textChanged = true;
            //var textbox = (sender as RichTextBox);
            //TextRange range = new TextRange(textbox.Document.ContentStart, textbox.Document.ContentEnd);
            //string fixedText = string.Empty;
            //outputText.Text = lexText(range.Text, ref fixedText);
            ////fixedTextBlock.Text = fixedText;
            ///


            var textbox = (sender as RichTextBox);
            TextRange range = new TextRange(textbox.Document.ContentStart, textbox.Document.ContentEnd);
 
            Regex regex = new Regex(@"\""[^""]*\""");
            Regex regex2 = new Regex(@"(199[89])|(200[0-4])");
            Regex regex3 = new Regex(@"(?=.*[0-9])(?=.*[#?!|\@\$%\^&*\-_])(?=.*[а-я])(?=.*[А-Я])[0-9а-яА-Я#?!|\@\$%\^&*\-_]{8,}");


            outputText.Text = string.Empty;
            MatchCollection matches = regex.Matches(range.Text);
            if (matches.Count > 0)
            {
                foreach (Match match in matches)
                {
                    outputText.Text += "Подстрока: " + match.Value + " ";
                    outputText.Text += "Начальная позиция: "+ match.Index + "\r\n";
                }
            }
            matches = regex2.Matches(range.Text);
            {
                foreach (Match match in matches)
                {
                    outputText.Text += "Подстрока: " + match.Value + " ";
                    outputText.Text += "Начальная позиция: " + match.Index + "\r\n";
                }
            }

            matches = regex3.Matches(range.Text);
            {
                foreach (Match match in matches)
                {
                    outputText.Text += "Подстрока: " + match.Value + " ";
                    outputText.Text += "Начальная позиция: " + match.Index + "\r\n";
                }
            }
        }




        string lexText(string text, ref string fixedText)
        {

            StringBuilder finalTextString = new StringBuilder();
            StringBuilder finalFixedTextString = new StringBuilder(fixedText);
            States tempState = States.None;
            Parser parser = new Parser();
            List<Error> errors = FindErrors(text, finalTextString, ref parser,finalFixedTextString);



            int count = 0;
            bool flag = false;
            bool flag2 = false;
            int number = 0;
            if (errors.Count > 0)
            {
                tempState = errors[0].PreviousState;
            }
            
            else
            {
                tempState = parser.CurrentState;
            }
            while (flag != true)
            {
                //errors[0].PreviousState = tempState;
                for (int i = number; i < errors.Count; i++)
                {
                    if (parser.MatchToken(errors[i].Token, tempState) != States.ERROR)
                    {
                        number = i+1;
                        flag2 = true;
                        finalFixedTextString.Append(" ");
                        finalFixedTextString.Append(errors[i].Text);
                        break;
                    }
                }
                if (tempState >= States.ERROR)
                {
                    flag = true;
                }
                else if (flag2 == false)
                {
                    if (errors.Count > 0)
                    {
                        finalTextString.Append( "Ошибка: " +
                               " line: " + errors[0].Current_line + " Ожидаемый символ: \"" + tempState + " \"" + "\n");
                        count++;
                    }
                    if(tempState == States.None && errors.Count <=0)
                    {
                        break;
                    }
                    else if(errors.Count <=0)
                    {
                        finalTextString.Append("Ошибка: " +
                            " line: " + " Ожидаемый символ: \"" + tempState + " \"" + "\n");
                        count++;
                    }
                    
                }

                tempState++;
                flag2 = false;
            }
            if (count == 0)
            {
                return "Ошибок нет";
            }
            fixedText = finalFixedTextString.ToString();
            return finalTextString.ToString();
        }
        

        List<Error> FindErrors(string text, StringBuilder finalText, ref Parser parser, StringBuilder fixedText)
        {
            string temp = string.Empty;
            int current_line = 0;
            int start_pos = 0;
            int end_pos = 0;

            Token currentToken = lexer(text[0].ToString());
            Token tempToken;

            List<Error> errors = new List<Error>();
            States tempState = States.None;
            for (int i = 0; i < text.Length; i++)
            {
                tempToken = lexer(text[i].ToString());
                if (currentToken.Type == TokenType.TOKEN_IDENTIFIER && tempToken.Type == TokenType.TOKEN_ERROR)
                {
                    tempToken = currentToken;
                }
                if (tempToken.Type != currentToken.Type || tempToken.Type == TokenType.TOKEN_WHITESPACE)
                {
                    currentToken = lexer(temp);
                    end_pos--;

                    tempState = parser.Parse(currentToken.Type);
                    if (tempState == States.ERROR)
                    {
                        tempState = parser.ParseError(currentToken.Type);
                        if (tempState == States.ERROR)
                        {
                            errors.Add(new Error(start_pos, end_pos, temp, currentToken.Type, current_line, parser.PreviousState));
                        }
                        else if (tempState != States.Whitespace)
                        {
                            for (int j = 0; j < errors.Count; j++)
                            {
                                finalText.Append("Ошибка: " + errors[j].Token + " - " + errors[j].Text + " - " + " position" +
                             " [" + errors[j].Start_pos + "," + errors[j].End_pos + "]" + " line: " + errors[j].Current_line + "\n");
                            }
                            fixedText.Append(temp);
                            errors.Clear();
                        }
                    }
                    else if(tempState == States.None)
                    {
                        fixedText.Remove(0,fixedText.Length);
                    }
                    else if(temp !="\n" && temp!="\r" && errors.Count <1)
                    {
                        fixedText.Append(temp);
                    }

                    if (temp == "\n")
                    {
                        current_line++;
                        start_pos = 0;
                        end_pos = 0;
                        temp = string.Empty;
                        currentToken = tempToken;
                    }
                    else
                    {
                        end_pos++;
                        start_pos = end_pos;
                        temp = string.Empty;
                        currentToken = tempToken;
                    }
                }
                //current_error = false;
                temp += text[i];
                end_pos++;
            }

            currentToken = lexer(temp);
            end_pos--;
            tempState = parser.Parse(currentToken.Type);
            if (tempState == States.ERROR)
            {
                tempState = parser.ParseError(currentToken.Type);
                if (tempState == States.ERROR)
                {
                    errors.Add(new Error(start_pos, end_pos, temp, currentToken.Type, current_line, parser.PreviousState));
                }
                else if (tempState != States.Whitespace)
                {
                    for (int j = 0; j < errors.Count; j++)
                    {
                        finalText.Append( "Ошибка: " + errors[j].Token + " - " + errors[j].Text + " - " + " position" +
                     " [" + errors[j].Start_pos + "," + errors[j].End_pos + "]" + " line: " + errors[j].Current_line + "\n");
                    }
                    fixedText.Append(temp);
                    errors.Clear();
                }
            }
            else if (tempState == States.None)
            {
                fixedText.Remove(0, fixedText.Length);
            }
            else if (temp != "\n" && temp != "\r" && errors.Count < 1)
            {
                fixedText.Append(temp);
            }

            return errors;
        }
        Token lexer(string strToLex)
        {

            switch (strToLex)
            {
                case "int": return new Token("ключевое слово", TokenType.TOKEN_INT);
                case "string": return new Token("ключевое слово", TokenType.TOKEN_STRING);
                case "Dictionary": return new Token("ключевое слово", TokenType.TOKEN_DICTIONARY);
                case "new": return new Token("ключевое слово", TokenType.TOKEN_NEW);
                case "=": return new Token("оператор присваивания", TokenType.TOKEN_EQUALS);
                case ";": return new Token("конец оператора", TokenType.TOKEN_SEMICOLON);
                case "<": return new Token("открывающая треугольная скобка", TokenType.TOKEN_LEFT_ANGLE_BRACKET);
                case ">": return new Token("закрывающая треугольная скобка", TokenType.TOKEN_RIGHT_ANGLE_BRACKET);
                case "(": return new Token("открывающая скобка", TokenType.TOKEN_LEFT_PARANTHESES);
                case ")": return new Token("закрывающая скобка", TokenType.TOKEN_RIGHT_PARANTHESES);
                case ",": return new Token("запятая", TokenType.TOKEN_COMMA);
                case " ": return new Token("разделитель", TokenType.TOKEN_WHITESPACE);
                case "\r": return new Token("разделитель", TokenType.TOKEN_WHITESPACE_R);
                case "\n": return new Token("разделитель", TokenType.TOKEN_WHITESPACE_N);
                default: break;
            }
            Regex regex = new Regex("[A-Za-z_]([A-Za-z_]|[0-9])*");
            Match match = regex.Match(strToLex);
            string ident = match.Value;
            if (ident != string.Empty)
            {
                return new Token("идентификатор", TokenType.TOKEN_IDENTIFIER);
            }
            return new Token("недопустимый символ", TokenType.TOKEN_ERROR);
        }
    }
}