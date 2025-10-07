using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace RIS_p2_LR3
{
    /// <summary>
    /// Handles file storage operations for departments data
    /// </summary>
    public class FileStorage
    {
        private readonly string _filePath;

        public FileStorage(string filePath = "departments.json")
        {
            _filePath = filePath;
        }

        /// <summary>
        /// Saves departments list to file
        /// </summary>
        /// <param name="departments">List of departments to save</param>
        /// <returns>True if successful, false otherwise</returns>
        public bool SaveDepartments(List<Department> departments)
        {
            try
            {
                var lines = new List<string>();
                foreach (var department in departments)
                {
                    lines.Add($"{department.Id}|{department.Name}|{department.Description}|{department.EmployeeCount}|{department.CreatedDate:yyyy-MM-dd HH:mm:ss}|{department.LastModifiedDate:yyyy-MM-dd HH:mm:ss}");
                }
                File.WriteAllLines(_filePath, lines, Encoding.UTF8);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при сохранении данных: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Loads departments list from file
        /// </summary>
        /// <returns>List of departments or empty list if file doesn't exist or error occurs</returns>
        public List<Department> LoadDepartments()
        {
            try
            {
                if (!File.Exists(_filePath))
                {
                    return new List<Department>();
                }

                var lines = File.ReadAllLines(_filePath, Encoding.UTF8);
                var departments = new List<Department>();

                foreach (var line in lines)
                {
                    if (string.IsNullOrWhiteSpace(line))
                        continue;

                    var parts = line.Split('|');
                    if (parts.Length == 6)
                    {
                        var department = new Department
                        {
                            Id = int.Parse(parts[0]),
                            Name = parts[1],
                            Description = parts[2],
                            EmployeeCount = int.Parse(parts[3]),
                            CreatedDate = DateTime.Parse(parts[4]),
                            LastModifiedDate = DateTime.Parse(parts[5])
                        };
                        departments.Add(department);
                    }
                }

                return departments;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при загрузке данных: {ex.Message}");
                return new List<Department>();
            }
        }

        /// <summary>
        /// Checks if data file exists
        /// </summary>
        /// <returns>True if file exists, false otherwise</returns>
        public bool FileExists()
        {
            return File.Exists(_filePath);
        }

        /// <summary>
        /// Creates backup of current data file
        /// </summary>
        /// <returns>True if backup created successfully, false otherwise</returns>
        public bool CreateBackup()
        {
            try
            {
                if (!File.Exists(_filePath))
                {
                    return false;
                }

                string backupPath = $"{_filePath}.backup_{DateTime.Now:yyyyMMdd_HHmmss}";
                File.Copy(_filePath, backupPath);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при создании резервной копии: {ex.Message}");
                return false;
            }
        }
    }
}
