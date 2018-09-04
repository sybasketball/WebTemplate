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
    /// FileTools �ļ��๤��
    /// </summary>
    public class FileTools
    {
        public const string UploadPath = "UpdateFiles\\";

        #region �ļ�����
        /// <summary>
        /// Create File to localhost
        /// </summary>
        /// <param name="Context">File Context</param>
        /// <param name="Path">Save Path(example:C:\tempfile)</param>
        /// <param name="FileExtendMame">ExtendName(example:exe��bmp)</param>
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
        /// �����ļ�
        /// </summary>
        /// <param name="path">�ļ�·��</param>
        /// <param name="content">�ļ�����</param>
        public static void Create(string path, byte[] content)
        {
            FileTools.EnsurePath(Path.GetDirectoryName(path));
            FileStream stream1 = new FileStream(path, FileMode.OpenOrCreate);
            stream1.Write(content, 0, content.Length);
            stream1.Close();
        }

        /// <summary>
        /// �����ļ�
        /// </summary>
        /// <param name="path">�ļ�·��</param>
        /// <param name="content">�ļ�����</param>
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
        /// �����ļ�
        /// </summary>
        /// <param name="path">�ļ�·��</param>
        /// <param name="content">�ļ�����</param>
        /// <param name="encoding">���ݱ���</param>
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
        /// �½���������ļ������������
        /// </summary>
        /// <param name="path">�ļ�·��</param>
        /// <param name="content">�ļ�����</param>
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
        /// �½���������ļ������������
        /// </summary>
        /// <param name="path">�ļ�·��</param>
        /// <param name="content">�ļ�����</param>
        /// <param name="encoding">���ݱ���</param>
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
        /// ɾ���ļ�
        /// </summary>
        /// <param name="path">�ļ�·��</param>
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
        /// �ж�·���Ƿ���ڣ�����������򴴽���·��Ŀ¼�ṹ
        /// </summary>
        /// <param name="path">·��</param>
        private static void EnsurePath(string path)
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
        }

        /// <summary>
        /// �ж�·���Ƿ���ڣ�����������򴴽���·��Ŀ¼�ṹ
        /// </summary>
        /// <param name="path">·��</param>
        private static void EnsureFilePath(string path)
        {
            int index = path.LastIndexOf("\\");
            if(index > 0)
                path = path.Substring(0,index);
            EnsurePath(path);
        }

        /// <summary>
        /// �ж��ļ��Ƿ����
        /// </summary>
        /// <param name="path">�ļ�·��</param>
        /// <returns></returns>
        public static bool Exists(string path)
        {
            return File.Exists(path);
        }

        /// <summary>
        /// �ж���չ���Ƿ���ͬ
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
        /// ��ȡָ���ļ�������
        /// </summary>
        /// <param name="path">�ļ�·��</param>
        /// <returns>���������ı�����</returns>
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
        /// ��ȡָ���ļ���ȫ������
        /// </summary>
        /// <param name="Path">�ļ�·��</param>
        /// <returns>�ļ�����</returns>
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
        /// ��������Ƹ�������_Ĭ��Ϊ����
        /// </summary>
        /// <param name="rs">HttpResponse����</param>
        /// <param name="attach">��������</param>
        /// <param name="title">�ļ���</param>
        /// <param name="extname">��չ��</param>
        /// <param name="fsize">�ļ���С</param>
        public static void ResponseAttach(HttpResponse rs, byte[] attach, string title,
            string extname, int fsize)
        {
            rs.ContentType = "application/octet-stream";
            rs.AddHeader("Content-Disposition", "attachment;FileName=" + HttpUtility.UrlEncode(title, System.Text.Encoding.Default) + "." + extname);
            rs.OutputStream.Write(attach, 0, fsize);
            rs.End();
        }
        /// <summary>
        /// ��������Ƹ�������_Ĭ��Ϊ����
        /// </summary>
        /// <param name="rs">HttpResponse����</param>
        /// <param name="attach">��������</param>
        /// <param name="fileName">�����ļ���</param>
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
        /// �ϴ������Ƹ�������
        /// </summary>
        /// <param name="uploadFile">�ͻ����ļ�</param>
        /// <param name="attach">�����ļ�������</param>
        ///<param name="fullName">�ļ���</param>
        /// <param name="extendName">�ļ���չ��</param>
        /// <param name="fSize">�ļ���С</param>
        public static void UploadAttach(HttpPostedFile uploadFile, ref byte[] attach, ref string fullName,
            ref string extendName, ref float fSize)
        {
            if (uploadFile.ContentLength > 0)
            {
                string nam = uploadFile.FileName;
                //��ȡ�ļ���(����·��)
                int n = nam.LastIndexOf(@"\");
                int i = nam.LastIndexOf(".");
                //��ȡ�ļ���
                fullName = nam.Substring(n + 1);
                //��ȡ�ļ���չ��
                if (i > 0)
                {
                    extendName = nam.Substring(i + 1);
                }
                else
                {
                    extendName = "";
                }
                //��ȡ�ļ���С
                fSize = uploadFile.ContentLength;

                //��ȡ�������ļ�
                attach = new byte[uploadFile.ContentLength];
                Stream StreamObject = uploadFile.InputStream;
                StreamObject.Read(attach, 0, uploadFile.ContentLength);
            }
        }


        /// <summary>
        /// �����ļ�����·����ȡ����������
        /// </summary>
        /// <param name="uploadFile">HttpPostedFile����</param>
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
        /// �����ļ�����·����ȡ����������
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
        /// ����ָ��Ŀ¼�������ļ���Ϣ
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
