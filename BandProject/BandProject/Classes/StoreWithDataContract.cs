using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BandProject
{
    public class StoreWithDataContract
    {
        public void deleteData(string filename)
        {
            using (IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (storage.FileExists(filename))
                {
                    storage.DeleteFile(filename);
                }
            }
        }
        public void SaveData(List<Sport> vList, string filename)
        {
            try
            {
                using (IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    if (storage.FileExists(filename))
                    {
                        storage.DeleteFile(filename);
                    }

                    using (var file = storage.CreateFile(filename))
                    {
                        var serializer = new DataContractSerializer(typeof(List<Sport>));
                        serializer.WriteObject(file, vList);
                    }
                }
            }
            catch (Exception)
            {
                //your clever error handling goes here
            }
        }

        public List<Sport> LoadData(string FileName)
        {
            try
            {
                using (IsolatedStorageFile isf = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    if (isf.FileExists(FileName))
                    {
                        var serializer = new DataContractSerializer(typeof(List<Sport>));
                        using (var file = isf.OpenFile(FileName, FileMode.Open))
                        {
                            return (List<Sport>)serializer.ReadObject(file);
                        }
                    }
                    else
                    {
                        return new List<Sport>();
                    }
                }
            }
            catch (Exception)
            {
                return new List<Sport>();
            }

        }

        public void SaveDataGesture(List<Gesture> vList, string filename)
        {
            try
            {
                using (IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    if (storage.FileExists(filename))
                    {
                        storage.DeleteFile(filename);
                    }

                    using (var file = storage.CreateFile(filename))
                    {
                        var serializer = new DataContractSerializer(typeof(List<Gesture>));
                        serializer.WriteObject(file, vList);
                    }
                }
            }
            catch (Exception)
            {
                //your clever error handling goes here
            }
        }

        public List<Gesture> LoadDataGesture(string FileName)
        {
            try
            {
                using (IsolatedStorageFile isf = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    if (isf.FileExists(FileName))
                    {
                        var serializer = new DataContractSerializer(typeof(List<Gesture>));
                        using (var file = isf.OpenFile(FileName, FileMode.Open))
                        {
                            return (List<Gesture>)serializer.ReadObject(file);
                        }
                    }
                    else
                    {
                        return new List<Gesture>();
                    }
                }
            }
            catch (Exception)
            {
                return new List<Gesture>();
            }

        }

        public void SaveDataSample(List<Sample> vList, string filename)
        {
            try
            {
                using (IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    if (storage.FileExists(filename))
                    {
                        storage.DeleteFile(filename);
                    }

                    using (var file = storage.CreateFile(filename))
                    {
                        var serializer = new DataContractSerializer(typeof(List<Sample>));
                        serializer.WriteObject(file, vList);
                    }
                }
            }
            catch (Exception)
            {
                //your clever error handling goes here
            }
        }

        public List<Sample> LoadDataSample(string FileName)
        {
            try
            {
                using (IsolatedStorageFile isf = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    if (isf.FileExists(FileName))
                    {
                        var serializer = new DataContractSerializer(typeof(List<Sample>));
                        using (var file = isf.OpenFile(FileName, FileMode.Open))
                        {
                            return (List<Sample>)serializer.ReadObject(file);
                        }
                    }
                    else
                    {
                        return new List<Sample>();
                    }
                }
            }
            catch (Exception)
            {
                return new List<Sample>();
            }

        }

        public void SaveDataPoint(List<Point> vList, string filename)
        {
            try
            {
                using (IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    if (storage.FileExists(filename))
                    {
                        storage.DeleteFile(filename);
                    }

                    using (var file = storage.CreateFile(filename))
                    {
                        var serializer = new DataContractSerializer(typeof(List<Point>));
                        serializer.WriteObject(file, vList);
                    }
                }
            }
            catch (Exception)
            {
                //your clever error handling goes here
            }
        }

        public List<Point> LoadDataPoint(string FileName)
        {
            try
            {
                using (IsolatedStorageFile isf = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    if (isf.FileExists(FileName))
                    {
                        var serializer = new DataContractSerializer(typeof(List<Point>));
                        using (var file = isf.OpenFile(FileName, FileMode.Open))
                        {
                            return (List<Point>)serializer.ReadObject(file);
                        }
                    }
                    else
                    {
                        return new List<Point>();
                    }
                }
            }
            catch (Exception)
            {
                return new List<Point>();
            }

        }
    }
}
