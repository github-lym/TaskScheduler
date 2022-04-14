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
            string logName;
            string logFullPath;
            string pathNow;
            string output;

            try
            {
                pathNow = Directory.GetCurrentDirectory();
                TaskService taskService = new TaskService();
                TaskCollection taskCollection = taskService.GetFolder(@"\").GetTasks();
                string[] taskName;

                if (Properties.Settings.Default.TaskName != null && Properties.Settings.Default.TaskName != string.Empty && taskCollection.Count != 0)
                {
                    output = Properties.Settings.Default.Output.ToString().Trim();
                    taskName = Properties.Settings.Default.TaskName.Split(',');
                   
                    if (taskName != null && taskName.Length != 0)
                    {
                        logName = "排程log.txt";   //以當時為名的log

                        StringBuilder sb = new StringBuilder();
                        sb.AppendLine($"排程執行時間:{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}");
                        sb.AppendLine("");
                        for (int i = 0; i < taskName.Length; i++)
                        {
                            for (int j = 0; j < taskCollection.Count; j++)
                            {
                                if (taskCollection[j].Name.Equals(taskName[i]))
                                {
                                    sb.AppendLine($"排程:{taskCollection[j].Name},執行狀態:{taskCollection[j].LastTaskResult}");
                                    sb.AppendLine($"最後執行時間:{taskCollection[j].LastRunTime},下次執行時間:{taskCollection[j].NextRunTime.ToString()}");
                                    sb.AppendLine("");
                                }
                            }

                            logFullPath = Path.Combine(output, logName);
                            using (StreamWriter sw = new StreamWriter(logFullPath, false))
                            {
                                sw.Write(sb.ToString());
                            }

                        }
                    }

                }
            }
            catch (Exception ex)
            {
                logName = "err.txt";
                logFullPath = Path.Combine(Properties.Settings.Default.Output.ToString().Trim(), logName);
                using (StreamWriter sw = new StreamWriter(logFullPath, false))
                {
                    sw.Write(ex.ToString());
                }
            }
            
        }
    }
}