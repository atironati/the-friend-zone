﻿<%@ WebHandler Language="C#" Class="ImageHandler2" %>
 
using System;
using System.Configuration;
using System.Web;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
 
public class ImageHandler2 : IHttpHandler
{
    protected SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ajt"].ConnectionString);

    public void ProcessRequest(HttpContext context)
    {
       Int32 id;
       Int32 photoId = 0;
       bool photoIdSet = false;
       Int32 imageWidth;
       Int32 imageHeight;

       if (context.Request.QueryString["id"] != null)
           id = Convert.ToInt32(context.Request.QueryString["id"]);
       else
           throw new ArgumentException("No parameter specified");
        
        if(context.Request.QueryString["photoId"] != null){
            photoId = Convert.ToInt32(context.Request.QueryString["photoId"]);
            photoIdSet = true;
        }
        
       if (context.Request.QueryString["width"] != null && context.Request.QueryString["height"] != null)
       {
           imageWidth = Convert.ToInt32(context.Request.QueryString["width"]);
           imageHeight = Convert.ToInt32(context.Request.QueryString["height"]);

           context.Response.ContentType = "image/jpeg";
           String imgpath = "";
           if (photoIdSet)
           {
               imgpath = GetImagePath(photoId);
           }
           else
           {
               imgpath = GetMainImagePath(id);
           }

           Image tempImage = NonLockingOpen(HttpContext.Current.Server.MapPath(imgpath));
           Bitmap loBMP = new Bitmap(tempImage);
           tempImage.Dispose();
           Bitmap bmpOut = new Bitmap(imageWidth, imageHeight);

           Graphics g = Graphics.FromImage(bmpOut);
           g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
           g.FillRectangle(Brushes.White, 0, 0, imageWidth, imageHeight);
           g.DrawImage(loBMP, 0, 0, imageWidth, imageHeight);
           MemoryStream ms = new MemoryStream();
           bmpOut.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
           byte[] bmpBytes = ms.GetBuffer();
           bmpOut.Dispose();
           ms.Close();
           context.Response.BinaryWrite(bmpBytes); 
       }
       else if (context.Request.QueryString["maxWidth"] != null && context.Request.QueryString["maxHeight"] != null)
       {
           imageWidth = Convert.ToInt32(context.Request.QueryString["maxWidth"]);
           imageHeight = Convert.ToInt32(context.Request.QueryString["maxHeight"]);

           context.Response.ContentType = "image/jpeg";
           String imgpath = "";
           if (photoIdSet)
           {
               imgpath = GetImagePath(photoId);
           } else {
               imgpath = GetMainImagePath(id);
           }

           // (Bitmap)System.Drawing.Bitmap.FromFile(HttpContext.Current.Server.MapPath(imgpath))
           // Image.FromFile(HttpContext.Current.Server.MapPath(imgpath))
           
           Image tempImage = NonLockingOpen(HttpContext.Current.Server.MapPath(imgpath));
           Bitmap loBMP = new Bitmap(tempImage);
           tempImage.Dispose();
           Bitmap bmpOut = new Bitmap(imageWidth, imageHeight);
           bmpOut = ProportionallyResizeBitmap(loBMP, imageWidth, imageHeight);
           loBMP.Dispose();
           
           MemoryStream ms = new MemoryStream();
           bmpOut.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
           byte[] bmpBytes = ms.GetBuffer();
           bmpOut.Dispose();
           ms.Close();
           context.Response.BinaryWrite(bmpBytes); 
       }

    }

    public String GetImagePath(int photo_id)
    {
        string sql = "SELECT photo FROM ajt.photos WHERE photo_id = @photo_id";
        SqlCommand cmd = new SqlCommand(sql,connection);
        cmd.CommandType = CommandType.Text;
        cmd.Parameters.AddWithValue("@photo_id", photo_id);
        connection.Open();
        String imgpath = (String)cmd.ExecuteScalar();
        connection.Close();
        return imgpath;
    }

    public String GetMainImagePath(int id)
    {
        string sql = "SELECT main_photo FROM ajt.profile_info WHERE user_id = @id";
        SqlCommand cmd = new SqlCommand(sql, connection);
        cmd.CommandType = CommandType.Text;
        cmd.Parameters.AddWithValue("id", id);
        connection.Open();
        String imgpath = (String)cmd.ExecuteScalar();
        connection.Close();
        return imgpath;
    }
 
    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

    public static Image NonLockingOpen(string filename)
    {
        Image result;

        #region Save file to byte array

        long size = (new FileInfo(filename)).Length;
        FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read);
        byte[] data = new byte[size];
        try
        {
            fs.Read(data, 0, (int)size);
        }
        finally
        {
            fs.Close();
            fs.Dispose();
        }

        #endregion

        #region Convert bytes to image

        MemoryStream ms = new MemoryStream();
        ms.Write(data, 0, (int)size);
        result = new Bitmap(ms);
        ms.Close();

        #endregion

        return result;
    }
    
    public Bitmap ProportionallyResizeBitmap(Bitmap src, int maxWidth, int maxHeight)
    {
        // original dimensions
        int w = src.Width;
        int h = src.Height;

        // Longest and shortest dimension
        int longestDimension = (w > h) ? w : h;
        int shortestDimension = (w < h) ? w : h;

        // propotionality
        float factor = ((float)longestDimension) / shortestDimension;

        // default width is greater than height
        double newWidth = maxWidth;
        double newHeight = maxWidth / factor;

        // if height greater than width recalculate
        if (w < h)
        {
            newWidth = maxHeight / factor;
            newHeight = maxHeight;
        }

        // Create new Bitmap at new dimensions
        Bitmap result = new Bitmap((int)newWidth, (int)newHeight);
        using (Graphics g = Graphics.FromImage((System.Drawing.Image)result))
            g.DrawImage(src, 0, 0, (int)newWidth, (int)newHeight);

        return result;
    }
 
    
    
}