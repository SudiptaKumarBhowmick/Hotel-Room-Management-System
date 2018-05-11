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

        //Leave Guest
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

        //Remove Multiple User
        public bool removeMultipleUser(Guest_Info Guest)
        {
            int i = 0;
            SqlConnection con = new SqlConnection(config);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }

            //Hotel Info
            string command = "update Hotel_Info set IsBooked = @IsBooked WHERE Room_No IN(SELECT Room_No FROM Guest_Info WHERE Guest_name = @Guest_name and Guest_addr = @Guest_addr and Guest_phone = @Guest_phone and Country = @Country and City = @City and age = @age)";
            SqlCommand com = new SqlCommand(command, con);
            com.Parameters.AddWithValue("@IsBooked", false);
            com.Parameters.AddWithValue("@Guest_name", Guest.Guest_name);
            com.Parameters.AddWithValue("@Guest_addr", Guest.Guest_addr);
            com.Parameters.AddWithValue("@Guest_phone", Guest.Guest_phone);
            com.Parameters.AddWithValue("@Country", Guest.Country);
            com.Parameters.AddWithValue("@City", Guest.City);
            com.Parameters.AddWithValue("@age", Guest.age);
            i += com.ExecuteNonQuery();
            com.Parameters.Clear();


            //Guest Info
            string command1 = "update Guest_Info set Room_No = @Room_No where Guest_name = @Guest_name and Guest_addr = @Guest_addr and Guest_phone = @Guest_phone and Country = @Country and City = @City and age = @age";
            SqlCommand cmd = new SqlCommand(command1, con);
            cmd.Parameters.AddWithValue("@Room_No", DBNull.Value);
            cmd.Parameters.AddWithValue("@Guest_name", Guest.Guest_name);
            cmd.Parameters.AddWithValue("@Guest_addr", Guest.Guest_addr);
            cmd.Parameters.AddWithValue("@Guest_phone", Guest.Guest_phone);
            cmd.Parameters.AddWithValue("@Country", Guest.Country);
            cmd.Parameters.AddWithValue("@City", Guest.City);
            cmd.Parameters.AddWithValue("@age", Guest.age);
            i += cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();


            //AllGuestInfo
            string command2 = "update AllGuestInfo set IsBooked = @IsBooked where Guest_name = @Guest_name and Guest_addr = @Guest_addr and Guest_phone = @Guest_phone and Country = @Country and City = @City and age = @age";
            SqlCommand cmd1 = new SqlCommand(command2, con);
            cmd1.Parameters.AddWithValue("@IsBooked", false);
            cmd1.Parameters.AddWithValue("@Guest_name", Guest.Guest_name);
            cmd1.Parameters.AddWithValue("@Guest_addr", Guest.Guest_addr);
            cmd1.Parameters.AddWithValue("@Guest_phone", Guest.Guest_phone);
            cmd1.Parameters.AddWithValue("@Country", Guest.Country);
            cmd1.Parameters.AddWithValue("@City", Guest.City);
            cmd1.Parameters.AddWithValue("@age", Guest.age);
            i += cmd1.ExecuteNonQuery();
            cmd1.Parameters.Clear();


            con.Close();

            if (i<3)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public void AddRoom(Hotel_Info hotel)
        {
            using (SqlConnection con = new SqlConnection(config))
            {
                string command = "Insert into Hotel_Info(Room_No, Room_type, IsBooked) values(@Room_No,@Room_type,@IsBooked)";
                SqlCommand cmd = new SqlCommand(command, con);

                SqlParameter paramNo = new SqlParameter();
                paramNo.ParameterName = "@Room_No";
                paramNo.Value = hotel.Room_No;
                cmd.Parameters.Add(paramNo);

                SqlParameter paramType = new SqlParameter();
                paramType.ParameterName = "@Room_type";
                paramType.Value = hotel.Room_type;
                cmd.Parameters.Add(paramType);

                SqlParameter paramBook = new SqlParameter();
                paramBook.ParameterName = "@IsBooked";
                paramBook.Value = hotel.IsBooked;
                cmd.Parameters.Add(paramBook);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void EditRoom(Hotel_Info hotel)
        {
            using (SqlConnection con = new SqlConnection(config))
            {
                string command = "update Hotel_Info set Room_type=@Room_type, IsBooked=@IsBooked where Room_No=@Room_No";
                SqlCommand cmd = new SqlCommand(command, con);

                SqlParameter paramNo = new SqlParameter();
                paramNo.ParameterName = "@Room_No";
                paramNo.Value = hotel.Room_No;
                cmd.Parameters.Add(paramNo);

                SqlParameter paramType = new SqlParameter();
                paramType.ParameterName = "@Room_type";
                paramType.Value = hotel.Room_type;
                cmd.Parameters.Add(paramType);

                SqlParameter paramBook = new SqlParameter();
                paramBook.ParameterName = "@IsBooked";
                paramBook.Value = hotel.IsBooked;
                cmd.Parameters.Add(paramBook);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }


        public void LevelTwoAuthenticateUser(string UserName, string password, string Email, int UserID)
        {
            using (SqlConnection con = new SqlConnection(config))
            {
                string command = "Insert into AuthenticateUser(UserName, password, Email, UserLevel, User_ID) values(@UserName, @password, @Email, @UserLevel, @User_ID)";
                SqlCommand com = new SqlCommand(command, con);

                SqlParameter paramUserName = new SqlParameter();
                paramUserName.ParameterName = "@UserName";
                paramUserName.Value = UserName;
                com.Parameters.Add(paramUserName);

                SqlParameter paramPassword = new SqlParameter();
                paramPassword.ParameterName = "@password";
                paramPassword.Value = password;
                com.Parameters.Add(paramPassword);

                SqlParameter paramEmail = new SqlParameter();
                paramEmail.ParameterName = "@Email";
                paramEmail.Value = Email;
                com.Parameters.Add(paramEmail);

                SqlParameter paramUserLevel = new SqlParameter();
                paramUserLevel.ParameterName = "@UserLevel";
                paramUserLevel.Value = "Level2";
                com.Parameters.Add(paramUserLevel);

                SqlParameter paramUserID = new SqlParameter();
                paramUserID.ParameterName = "@User_ID";
                paramUserID.Value = UserID;
                com.Parameters.Add(paramUserID);

                con.Open();
                com.ExecuteNonQuery();
            }
        }

        //Add value GuestInfo Table
        public void addAllGuestInfo(int Room_No)
        {
            int Guest_Id, age, RoomNo;
            string Guest_name, Guest_addr, Guest_phone, Country, City, Room_type;
            Boolean IsBooked;
            using (SqlConnection con = new SqlConnection(config))
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                string command = "select * from Guest_Info where Room_No = @Room_No";
                SqlCommand cmd = new SqlCommand(command, con);
                SqlParameter parameterRoomNo = new SqlParameter();
                parameterRoomNo.ParameterName = "@Room_No";
                parameterRoomNo.Value = Room_No;
                cmd.Parameters.Add(parameterRoomNo);

                SqlDataReader reader = cmd.ExecuteReader();
                reader.Read();
                Guest_Id = reader.GetInt32(0);
                Guest_name = reader.GetString(1);
                Guest_addr = reader.GetString(2);
                Guest_phone = reader.GetString(3);
                Country = reader.GetString(4);
                City = reader.GetString(5);
                age = reader.GetInt32(6);
                RoomNo = reader.GetInt32(7);

                reader.Close();

                string command1 = "select * from Hotel_Info where Room_No = @Room_No";
                SqlCommand sqlcommand = new SqlCommand(command1, con);
                SqlParameter paramRoomNo = new SqlParameter();
                paramRoomNo.ParameterName = "@Room_No";
                paramRoomNo.Value = Room_No;
                sqlcommand.Parameters.Add(paramRoomNo);

                SqlDataReader Reader = sqlcommand.ExecuteReader();
                Reader.Read();
                Room_type = Reader.GetString(1);
                IsBooked = Reader.GetBoolean(2);
                Reader.Close();

                command = "Insert into AllGuestInfo(Guest_Name, Guest_addr, Guest_phone, Country, City, age, Room_type, Guest_Id, Room_No, IsBooked) values (@Guest_Name, @Guest_addr, @Guest_phone, @Country, @City, @age, @Room_type, @Guest_Id, @Room_No, @IsBooked)";
                SqlCommand sqlcmd = new SqlCommand(command, con);

                SqlParameter paramName = new SqlParameter();
                paramName.ParameterName = "@Guest_Name";
                paramName.Value = Guest_name;
                sqlcmd.Parameters.Add(paramName);

                SqlParameter paramAddress = new SqlParameter();
                paramAddress.ParameterName = "@Guest_addr";
                paramAddress.Value = Guest_addr;
                sqlcmd.Parameters.Add(paramAddress);

                SqlParameter paramPhone = new SqlParameter();
                paramPhone.ParameterName = "@Guest_phone";
                paramPhone.Value = Guest_phone;
                sqlcmd.Parameters.Add(paramPhone);

                SqlParameter paramCountry = new SqlParameter();
                paramCountry.ParameterName = "@Country";
                paramCountry.Value = Country;
                sqlcmd.Parameters.Add(paramCountry);

                SqlParameter paramCity = new SqlParameter();
                paramCity.ParameterName = "@City";
                paramCity.Value = City;
                sqlcmd.Parameters.Add(paramCity);

                SqlParameter paramAge = new SqlParameter();
                paramAge.ParameterName = "@age";
                paramAge.Value = age;
                sqlcmd.Parameters.Add(paramAge);

                SqlParameter paramRoom_type = new SqlParameter();
                paramRoom_type.ParameterName = "@Room_type";
                paramRoom_type.Value = Room_type;
                sqlcmd.Parameters.Add(paramRoom_type);

                SqlParameter paramGuest_Id = new SqlParameter();
                paramGuest_Id.ParameterName = "@Guest_Id";
                paramGuest_Id.Value = Guest_Id;
                sqlcmd.Parameters.Add(paramGuest_Id);

                SqlParameter paramRoom_No = new SqlParameter();
                paramRoom_No.ParameterName = "@Room_No";
                paramRoom_No.Value = RoomNo;
                sqlcmd.Parameters.Add(paramRoom_No);

                SqlParameter paramIsBooked = new SqlParameter();
                paramIsBooked.ParameterName = "@IsBooked";
                paramIsBooked.Value = IsBooked;
                sqlcmd.Parameters.Add(paramIsBooked);

                sqlcmd.ExecuteNonQuery();
            }
        }

        //Multiple Booking For User
        public void MultipleBookingUser(FormCollection collection)
        {
            string Room_No = "";
            string[] valueArray = new string[100];

            foreach (string key in collection.AllKeys)
            {
                if (key == "Room_No")
                {
                    Room_No = collection[key];
                    valueArray = Room_No.Split(',');
                }
            }


            string[] collectionArray = new string[10];
            collectionArray[0] = collection["Guest_name"];
            collectionArray[1] = collection["Guest_addr"];
            collectionArray[2] = collection["Guest_phone"];
            collectionArray[3] = collection["Country"];
            collectionArray[4] = collection["City"];
            collectionArray[5] = collection["age"];


            using (SqlConnection con = new SqlConnection(config))
            {
                string command = "insert into Guest_Info (Guest_name, Guest_addr, Guest_phone, Country, City, age, Room_No) values(@Guest_name, @Guest_addr, @Guest_phone, @Country, @City, @age, @Room_No)";
                SqlCommand cmd = new SqlCommand(command, con);

                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }

                SqlParameter paramName = new SqlParameter();
                paramName.ParameterName = "@Guest_name";
                paramName.Value = collectionArray[0];
                cmd.Parameters.Add(paramName);

                SqlParameter paramAdd = new SqlParameter();
                paramAdd.ParameterName = "@Guest_addr";
                paramAdd.Value = collectionArray[1];
                cmd.Parameters.Add(paramAdd);

                SqlParameter paramPhone = new SqlParameter();
                paramPhone.ParameterName = "@Guest_phone";
                paramPhone.Value = collectionArray[2];
                cmd.Parameters.Add(paramPhone);

                SqlParameter paramCountry = new SqlParameter();
                paramCountry.ParameterName = "@Country";
                paramCountry.Value = collectionArray[3];
                cmd.Parameters.Add(paramCountry);

                SqlParameter paramCity = new SqlParameter();
                paramCity.ParameterName = "@City";
                paramCity.Value = collectionArray[4];
                cmd.Parameters.Add(paramCity);

                SqlParameter paramAge = new SqlParameter();
                paramAge.ParameterName = "@age";
                paramAge.Value = collectionArray[5];
                cmd.Parameters.Add(paramAge);

                SqlParameter paramRoomNo = new SqlParameter();
                paramRoomNo.ParameterName = "@Room_No";

                for (int i = 0; i < valueArray.Length; i++)
                {
                    paramRoomNo.Value = valueArray[i];
                    //cmd.Parameters.Add("@Room_No",SqlDbType.Int).Value = valueArray[i];
                    cmd.Parameters.Add(paramRoomNo);
                    cmd.ExecuteNonQuery();
                    cmd.Parameters.Remove(paramRoomNo);
                }

                string command2 = "update Hotel_Info set IsBooked = 1 where Room_No = @Room_No";
                SqlCommand com = new SqlCommand(command2, con);

                SqlParameter paramroomno = new SqlParameter();
                paramroomno.ParameterName = "@Room_No";

                for (int i = 0; i < valueArray.Length; i++)
                {
                    paramroomno.Value = valueArray[i];
                    com.Parameters.Add(paramroomno);
                    com.ExecuteNonQuery();
                    com.Parameters.Remove(paramroomno);
                }
            }

            //add All guest Info
            for (int i = 0; i < valueArray.Length; i++)
            {
                addAllGuestInfo(Convert.ToInt32(valueArray[i]));
            }
        }

        //Add New Admin
        public void LevelOneAuthenticateUser(string Name, string password, string Email, int Admin_ID)
        {
            using (SqlConnection con = new SqlConnection(config))
            {
                string command = "Insert into AuthenticateUser(UserName, password, Email, UserLevel, Admin_ID) values(@UserName, @password, @Email, @UserLevel, @Admin_ID)";
                SqlCommand com = new SqlCommand(command, con);

                SqlParameter paramUserName = new SqlParameter();
                paramUserName.ParameterName = "@UserName";
                paramUserName.Value = Name;
                com.Parameters.Add(paramUserName);

                SqlParameter paramPassword = new SqlParameter();
                paramPassword.ParameterName = "@password";
                paramPassword.Value = password;
                com.Parameters.Add(paramPassword);

                SqlParameter paramEmail = new SqlParameter();
                paramEmail.ParameterName = "@Email";
                paramEmail.Value = Email;
                com.Parameters.Add(paramEmail);

                SqlParameter paramUserLevel = new SqlParameter();
                paramUserLevel.ParameterName = "@UserLevel";
                paramUserLevel.Value = "Level1";
                com.Parameters.Add(paramUserLevel);

                SqlParameter paramAdminID = new SqlParameter();
                paramAdminID.ParameterName = "@Admin_ID";
                paramAdminID.Value = Admin_ID;
                com.Parameters.Add(paramAdminID);

                con.Open();
                com.ExecuteNonQuery();
            }
        }


    }
}