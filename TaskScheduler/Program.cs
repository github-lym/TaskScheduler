using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.Win32.TaskScheduler;

namespace TaskScheduler
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            TaskService taskService = new TaskService();
            TaskCollection taskCollection = taskService.GetFolder(@"\").GetTasks();
            string[] taskName;

            string logName;
            string logFullPath;
            StreamWriter log;

            if (Properties.Settings.Default.TaskName != null && Properties.Settings.Default.TaskName != string.Empty && taskCollection.Count != 0)
            {
                taskName = Properties.Settings.Default.TaskName.Split(',');
                if (taskName != null && taskName.Length != 0)
                {
                    string pathNow = Directory.GetCurrentDirectory();
                    string now = DateTime.Now.ToString("yyyyMMdd");
                    logName = now + ".log";   //以當時為名的log

                    if (!System.IO.Directory.Exists("TaskScheduler_log"))
                        System.IO.Directory.CreateDirectory("TaskScheduler_log");  //沒有資料夾就產生

                    logFullPath = Path.Combine("TaskScheduler_log", logName);

                    for (int i = 0; i < taskName.Length; i++)
                    {
                        for (int j = 0; j < taskCollection.Count; j++)
                        {
                            if (taskCollection[j].Name.Equals(taskName[i]))
                            {
                                if (!File.Exists(logFullPath))
                                    log = new StreamWriter(logFullPath);
                                else
                                    log = File.AppendText(logFullPath);

                                log.WriteLine("log產生時間{0}", DateTime.Now.ToString());
                                log.WriteLine("排程:{0},執行狀態:{1}", taskCollection[j].Name, taskCollection[j].LastTaskResult);
                                log.WriteLine("最後執行時間:{0},下次執行時間:{1}", taskCollection[j].LastRunTime, taskCollection[j].NextRunTime.ToString());
                                log.WriteLine("\n");
                                log.Flush();
                                log.Close();
                                log.Dispose();
                            }
                        }
                    }
                }
            }

            //Console.ReadKey();
        }
    }
}