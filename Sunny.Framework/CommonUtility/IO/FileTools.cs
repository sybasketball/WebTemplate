using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Collections;
using System.Data;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.UI;
using CommonUtility.Utility;

namespace CommonUtility.IO
{
    /// <summary>
    /// FileTools 文件类工具
    /// </summary>
    public class FileTools
    {
        public const string UploadPath = "UpdateFiles\\";

        #region 文件创建
        /// <summary>
        /// Create File to localhost
        /// </summary>
        /// <param name="Context">File Context</param>
        /// <param name="Path">Save Path(example:C:\tempfile)</param>
        /// <param name="FileExtendMame">ExtendName(example:exe、bmp)</param>
        /// <returns></returns>
        public static string CreateTempFile(byte[] Context, string Path, string FileExtendMame)
        {
            if (!Directory.Exists(Path)) Directory.CreateDirectory(Path);
            System.Threading.Thread.Sleep(10);
            string FullName = DateTime.Now.ToBinary().ToString() + "." + FileExtendMame;
            System.IO.FileStream fs = File.Create(Path + @"\" + FullName);
            fs.Write(Context, 0, Context.Length);
            fs.Flush();
            fs.Close();
            return FullName;
        }

        public static void CreateTempFile(string Path, byte[] Context, string FileName)
        {
            if (!Directory.Exists(Path))
            {
                Directory.CreateDirectory(Path);
            }
            System.Threading.Thread.Sleep(10);
            System.IO.FileStream fs = File.Create(Path + @"\" + FileName);
            fs.Write(Context, 0, Context.Length);
            fs.Flush();
            fs.Close();
        }

        /// <summary>
        /// 创建文件
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <param name="content">文件内容</param>
        public static void Create(string path, byte[] content)
        {
            FileTools.EnsurePath(Path.GetDirectoryName(path));
            FileStream stream1 = new FileStream(path, FileMode.OpenOrCreate);
            stream1.Write(content, 0, content.Length);
            stream1.Close();
        }

        /// <summary>
        /// 创建文件
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <param name="content">文件内容</param>
        public static void Create(string path, string content)
        {
            FileTools.EnsurePath(Path.GetDirectoryName(path));
            FileStream stream1 = new FileStream(path, FileMode.OpenOrCreate);
            StreamWriter writer1 = new StreamWriter(stream1);
            writer1.WriteLine(content);
            writer1.Close();
            stream1.Close();
        }

        /// <summary>
        /// 创建文件
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <param name="content">文件内容</param>
        /// <param name="encoding">内容编码</param>
        public static void Create(string path, string content, Encoding encoding)
        {
            FileTools.EnsurePath(Path.GetDirectoryName(path));
            FileStream stream1 = new FileStream(path, FileMode.OpenOrCreate);
            StreamWriter writer1 = new StreamWriter(stream1, encoding);
            writer1.WriteLine(content);
            writer1.Close();
            stream1.Close();
        }

        /// <summary>
        /// 新建或打开现有文件，并添加内容
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <param name="content">文件内容</param>
        public static void CreateAppend(string path, string content)
        {
            FileTools.EnsurePath(Path.GetFullPath(path));
            FileStream stream1 = new FileStream(path, FileMode.Append, FileAccess.Write);
            StreamWriter writer1 = new StreamWriter(stream1);
            writer1.WriteLine(content);
            writer1.Close();
            stream1.Close();
        }

        /// <summary>
        /// 新建或打开现有文件，并添加内容
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <param name="content">文件内容</param>
        /// <param name="encoding">内容编码</param>
        public static void CreateAppend(string path, string content, Encoding encoding)
        {
            FileTools.EnsurePath(Path.GetFullPath(path));
            FileStream stream1 = new FileStream(path, FileMode.Append, FileAccess.Write);
            StreamWriter writer1 = new StreamWriter(stream1, encoding);
            writer1.WriteLine(content);
            writer1.Close();
            stream1.Close();
        }
        #endregion

        #region Operate
        public static bool UploadFile(HttpPostedFile file,string fileName)
        {
            string folderPath = GetFullPath();
            return UploadFile(file,fileName,folderPath);
        }

        public static bool UploadFile(HttpPostedFile file,string fileName,string folderPath)
        {
            if (file != null)
            {
                if (string.IsNullOrEmpty(folderPath))
                    folderPath = GetFullPath(UploadPath);
                fileName = folderPath + fileName;
                EnsureFilePath(fileName);
                file.SaveAs(fileName);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="path">文件路径</param>
        public static void Delete(string path)
        {
            File.Delete(GetFullPath(path));
        }
        #endregion

        #region Get File Inf
        public static string GetFullPath(string path)
        {
            return GetFullPath() + path;
        }

        public static string GetFullPath()
        {
            return HttpContext.Current.Server.MapPath("~\\") + UploadPath;
        }

        public static string GetFileExtName(string filePath)
        {
            int index = filePath.LastIndexOf(".");
            return index > 0 ? filePath.Substring(index) : string.Empty;
        }

        public static string GetNewFileName(string filePath)
        {
            return GetNewFileName(filePath,Guid.NewGuid().ToString());
        }

        public static string GetNewFileName(string filePath, string newName)
        {
            if (!string.IsNullOrEmpty(filePath))
                return newName + GetFileExtName(filePath);
            else
                return string.Empty;
        }
        #endregion

        #region check
        /// <summary>
        /// 判断路径是否存在，如果不存在则创建该路径目录结构
        /// </summary>
        /// <param name="path">路径</param>
        private static void EnsurePath(string path)
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
        }

        /// <summary>
        /// 判断路径是否存在，如果不存在则创建该路径目录结构
        /// </summary>
        /// <param name="path">路径</param>
        private static void EnsureFilePath(string path)
        {
            int index = path.LastIndexOf("\\");
            if(index > 0)
                path = path.Substring(0,index);
            EnsurePath(path);
        }

        /// <summary>
        /// 判断文件是否存在
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <returns></returns>
        public static bool Exists(string path)
        {
            return File.Exists(path);
        }

        /// <summary>
        /// 判断扩展名是否相同
        /// </summary>
        /// <param name="file1"></param>
        /// <param name="file2"></param>
        /// <returns></returns>
        public static bool CheckExtName(string file1, string file2)
        {
            return GetFileExtName(file1) == GetFileExtName(file2);
        }
        #endregion






        /// <summary>
        /// 读取指定文件的内容
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <returns>按行排列文本内容</returns>
        public static string[] Read(string path)
        {
            string text1;
            ArrayList list1 = new ArrayList();
            FileStream stream1 = new FileStream(path, FileMode.Open);
            StreamReader reader1 = new StreamReader(stream1);
            while ((text1 = reader1.ReadLine()) != null)
            {
                list1.Add(text1);
            }
            reader1.Close();
            stream1.Close();
            return (string[])list1.ToArray(typeof(string));
        }

        /// <summary>
        /// 读取指定文件的全部内容
        /// </summary>
        /// <param name="Path">文件路径</param>
        /// <returns>文件内容</returns>
        public static string ReadALL(string Path)
        {
            ArrayList list1 = new ArrayList();
            FileStream stream1 = new FileStream(Path, FileMode.Open, FileAccess.Read, FileShare.Read);
            StreamReader reader1 = new StreamReader(stream1);

            string text1 = reader1.ReadToEnd();
            reader1.Close();
            stream1.Close();
            return text1;
        }

        /// <summary>
        /// 输出二进制附件内容_默认为下载
        /// </summary>
        /// <param name="rs">HttpResponse对象</param>
        /// <param name="attach">附件内容</param>
        /// <param name="title">文件名</param>
        /// <param name="extname">扩展名</param>
        /// <param name="fsize">文件大小</param>
        public static void ResponseAttach(HttpResponse rs, byte[] attach, string title,
            string extname, int fsize)
        {
            rs.ContentType = "application/octet-stream";
            rs.AddHeader("Content-Disposition", "attachment;FileName=" + HttpUtility.UrlEncode(title, System.Text.Encoding.Default) + "." + extname);
            rs.OutputStream.Write(attach, 0, fsize);
            rs.End();
        }
        /// <summary>
        /// 输出二进制附件内容_默认为下载
        /// </summary>
        /// <param name="rs">HttpResponse对象</param>
        /// <param name="attach">附件内容</param>
        /// <param name="fileName">完整文件名</param>
        public static void ResponseAttach(HttpResponse rs, byte[] attach, string fileName)
        {
            rs.ContentType = "application/octet-stream";
            rs.AddHeader("Content-Disposition", "attachment;FileName=" + HttpUtility.UrlEncode(fileName));
            if (attach != null && attach.Length > 0)
            {
                rs.OutputStream.Write(attach, 0, attach.Length);
                rs.End();
            }
        }

        /// <summary>
        /// DownLoad File to Client
        /// </summary>
        /// <param name="FileName"></param>
        /// <param name="Context"></param>
        /// <param name="page"></param>
        public static void DownLoadFile(string FileName, byte[] Context, Page page)
        {
            page.Response.ContentType = "application/octet-stream";
            if (FileName == "") FileName = "Temp";
            page.Response.AddHeader("Content-Disposition", "attachment;FileName=" + HttpUtility.UrlEncode(FileName));
            if (Context != null && Context.Length > 0)
                page.Response.OutputStream.Write(Context, 0, Context.Length);
            else
                page.Response.BinaryWrite(new byte[1]);
            page.Response.End();
        }

        /// <summary>
        /// 上传二进制附件内容
        /// </summary>
        /// <param name="uploadFile">客户端文件</param>
        /// <param name="attach">保存文件的数组</param>
        ///<param name="fullName">文件名</param>
        /// <param name="extendName">文件扩展名</param>
        /// <param name="fSize">文件大小</param>
        public static void UploadAttach(HttpPostedFile uploadFile, ref byte[] attach, ref string fullName,
            ref string extendName, ref float fSize)
        {
            if (uploadFile.ContentLength > 0)
            {
                string nam = uploadFile.FileName;
                //获取文件名(抱括路径)
                int n = nam.LastIndexOf(@"\");
                int i = nam.LastIndexOf(".");
                //获取文件名
                fullName = nam.Substring(n + 1);
                //获取文件扩展名
                if (i > 0)
                {
                    extendName = nam.Substring(i + 1);
                }
                else
                {
                    extendName = "";
                }
                //获取文件大小
                fSize = uploadFile.ContentLength;

                //获取二进制文件
                attach = new byte[uploadFile.ContentLength];
                Stream StreamObject = uploadFile.InputStream;
                StreamObject.Read(attach, 0, uploadFile.ContentLength);
            }
        }


        /// <summary>
        /// 根据文件物理路径获取二进制数据
        /// </summary>
        /// <param name="uploadFile">HttpPostedFile对象</param>
        /// <returns></returns>
        public static byte[] GetAttach(HttpPostedFile uploadFile)
        {
            byte[] attach = new byte[uploadFile.ContentLength];
            if (uploadFile.ContentLength > 0)
            {
                uploadFile.InputStream.Read(attach, 0, uploadFile.ContentLength);
            }
            return attach;
        }

        /// <summary>
        /// 本地文件物理路径获取二进制数据
        /// 2007-12-8_15:59:01_by_Jie
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static byte[] GetFileContent(string filePath)
        {
            try
            {
                if (!File.Exists(filePath))
                    return null;
                FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
                byte[] retVal = new byte[(int)fs.Length];

                fs.Read(retVal, 0, (int)fs.Length);
                fs.Flush();

                fs.Close();
                return retVal;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// 返回指定目录的所有文件信息
        /// </summary>
        /// <param name="Path"></param>
        /// <param name="searchPattern"></param>
        /// <returns></returns>
        public static List<FileInfo> GetFileInfoSet(string Path,string searchPattern)
        {
            List<FileInfo> result = new List<FileInfo>();

            DirectoryInfo di = new DirectoryInfo(Path);
            if (searchPattern != null)
               result.AddRange(di.GetFiles(searchPattern));
            else
               result.AddRange( di.GetFiles());
           return result;
        }

    }
}
