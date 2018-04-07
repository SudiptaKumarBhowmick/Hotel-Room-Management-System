using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using HotelRoomManagementSystem.Models;
using System.Data.Entity;

namespace HotelRoomManagementSystem.Common
{
    public class UserClass
    {
        string config = ConfigurationManager.ConnectionStrings["SampleDBContext"].ConnectionString;
        SampleDBContext db = new SampleDBContext();
        public bool removeUser(int Guest_Id)
        {
            SqlConnection con = new SqlConnection(config);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            string command = "update Guest_Info set Room_No = @Room_No where Guest_Id = @Guest_Id";
            SqlCommand cmd = new SqlCommand(command, con);
            cmd.Parameters.AddWithValue("@Room_No",DBNull.Value);
            cmd.Parameters.AddWithValue("@Guest_Id", Guest_Id);
            int i = cmd.ExecuteNonQuery();
            con.Close();
            if (i == 1)
                return true;
            else
                return false;
        }

    }
}