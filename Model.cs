using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Threading.Tasks;

namespace ToDo_App
{
    // приоритет задачи
    public enum Priority
    {
        Default, // по умолчанию (самый низкий)
        Low, // низкий
        Normal, // средний
        High // высокий
    }

    // действия для меню приложения
    public enum MenuAction
    {
        Default, 
        AddTask, // добавить задачу
        RemoveTask, // удалить задачу
        ChangeStatus, // изменить статус задачи
        SaveFile, // сохранить в файл
        SortList, // отсортировать по приоритету
        CloseProgram // закрыть программу
    }

    // задача
    public class ToDo_Task
    {
        public string TextTask { get; set; }
        public bool Status { get; set; }
        public Priority Prioritet { get; set; }

        public ToDo_Task() { 
            TextTask = string.Empty;
            Status = false;
            Prioritet = Priority.Default;
        }

        public ToDo_Task(string task) {
            TextTask = task;
            Status = false;
            Prioritet = Priority.Default;
        }

        public ToDo_Task(string task, Priority priority) : this(task)
        {
            Prioritet = priority;
        }

        public ToDo_Task(ToDo_Task source)
        {
            TextTask = source.TextTask;
            Status = source.Status;
            Prioritet = source.Prioritet;
        }

        public void ChangeStatus()
        {
            Status = !Status;
        }
    }

    // список
    public class ToDo_List
    {
        public List<ToDo_Task> ListNotCompleted { get; set; }
        public List<ToDo_Task> ListCompleted { get; set; }

        public ToDo_List() {
            ListCompleted = new List<ToDo_Task>();
            ListNotCompleted = new List<ToDo_Task>();
        }

        // кол-во элементов во ВСЕМ списке 
        public int ListCount
        {
            get
            {
                int count = 0;
                count += ListNotCompleted.Count;
                count += ListCompleted.Count;

                return count;
            }
        }

        // добавить новую задачу
        public void AddNewTask(ToDo_Task task) { 
            ListNotCompleted.Add(task);
        }

        // удалить задачу
        public void RemoveTask(int number) {
            // если номер задачи входит в список невыполненных
            if (number <= ListNotCompleted.Count) { 
                ListNotCompleted.RemoveAt(number-1);
            }
            else
            {
                number = number - ListNotCompleted.Count;
                ListCompleted.RemoveAt(number-1);
            }
        }

        // изменить статус задачи
        public void ChangeStatus(int number)
        {
            // если номер задачи входит в список невыполненных
            if (number <= ListNotCompleted.Count)
            {
                var _task = new ToDo_Task(ListNotCompleted[number-1]); // копируем задачу
                _task.ChangeStatus(); // меняем статус задачи
                ListCompleted.Add(_task); // добавляем в выполненные
                ListNotCompleted.RemoveAt(number - 1);
            }
            else
            {
                number = number - ListNotCompleted.Count; // номер в списке выполненных
                var _task = new ToDo_Task(ListCompleted[number - 1]); // копируем задачу
                _task.ChangeStatus(); // меняем статус задачи
                ListCompleted.RemoveAt(number - ListNotCompleted.Count - 1);
                ListNotCompleted.Add(_task); // добавляем в невыполненные
                
            }
        }

        public void SortLists()
        {
            ListCompleted = ListCompleted.OrderBy(x => x.Prioritet).ToList();
            ListNotCompleted = ListNotCompleted.OrderBy(x => x.Prioritet).ToList();
        }
    }

    // класс для работы с файлом
    public static class FileHandler
    {
        public static void SaveToFile(string filePath, ToDo_List data)
        {
            try
            {
                var settings = new JsonSerializerSettings
                {
                    Formatting = Formatting.Indented,
                    Converters = { new StringEnumConverter() }
                };

                string json = JsonConvert.SerializeObject(data, settings);
                File.WriteAllText(filePath, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка сохранения: {ex.Message}");
            }
        }

        public static ToDo_List LoadFromFile(string filePath)
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    Console.WriteLine("Файл не существует. Создан новый список.");
                    return new ToDo_List();
                }

                string json = File.ReadAllText(filePath);

                var settings = new JsonSerializerSettings
                {
                    Converters = { new StringEnumConverter() },
                    // Добавляем обработку отсутствующих значений
                    MissingMemberHandling = MissingMemberHandling.Ignore,
                    NullValueHandling = NullValueHandling.Ignore
                };

                var result = JsonConvert.DeserializeObject<ToDo_List>(json, settings);

                // Гарантируем инициализацию списков
                result.ListNotCompleted ??= new List<ToDo_Task>();
                result.ListCompleted ??= new List<ToDo_Task>();

                return result;
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Ошибка формата JSON: {ex.Message}");
                Console.WriteLine("Создан новый список.");
                return new ToDo_List();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Критическая ошибка: {ex.Message}");
                return new ToDo_List();
            }
        }
    }
}
