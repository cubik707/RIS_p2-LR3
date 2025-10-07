using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web.Script.Serialization;

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
                var jsonDepartments = departments.ConvertAll(d => DepartmentJson.FromDepartment(d));
                var serializer = new JavaScriptSerializer();
                string json = serializer.Serialize(jsonDepartments);
                File.WriteAllText(_filePath, json, Encoding.UTF8);
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

                string json = File.ReadAllText(_filePath, Encoding.UTF8);
                if (string.IsNullOrWhiteSpace(json))
                {
                    return new List<Department>();
                }

                var serializer = new JavaScriptSerializer();
                var jsonDepartments = serializer.Deserialize<List<DepartmentJson>>(json);
                
                if (jsonDepartments != null)
                {
                    return jsonDepartments.ConvertAll(jd => jd.ToDepartment());
                }
                
                return new List<Department>();
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
