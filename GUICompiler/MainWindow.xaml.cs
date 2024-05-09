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
            if (!textChanged) textChanged = true;
            var textbox = (sender as RichTextBox);
            TextRange range = new TextRange(textbox.Document.ContentStart, textbox.Document.ContentEnd);
            Parser parser = new Parser(range.Text);

            parser.Parse();
            outputText.Text = parser.Result;

        }



        
    }
}