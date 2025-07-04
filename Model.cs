using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

    // задача
    public class Task
    {
        string _textTask; // текст задачи
        bool _status; // статус выполнения
        Priority _prioritet; // приоритет выполнения

        public string TextTask { get { return _textTask; } set { _textTask = value; } }
        public bool Status { get { return _status; } set { _status = value; } }
        public Priority Prioritet { get { return _prioritet; } set { _prioritet = value; } }

        public Task() { 
            TextTask = string.Empty;
            Status = false;
            Prioritet = Priority.Default;
        }

        public Task(string task) {
            TextTask = task;
            Status = false;
            Prioritet = Priority.Default;
        }

        public Task(string task, Priority priority) : this(task)
        {
            Prioritet = priority;
        }
    }
}
