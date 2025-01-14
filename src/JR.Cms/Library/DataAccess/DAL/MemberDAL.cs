﻿//
// MemberDAL   会员数据访问
// Copryright 2011 @ fze.NET,All rights reserved !
// Create by newmin @ 2011/03/16
//

using System;
using System.Data;
using JR.Cms.Library.DataAccess.IDAL;
using JR.Stand.Core.Data;

namespace JR.Cms.Library.DataAccess.DAL
{
    /// <summary>
    /// 会员数据访问
    /// </summary>
    public sealed class MemberDal : DalBase, ImemberDAL
    {
        public bool DetectUserName(string username)
        {
            var obj = ExecuteScalar(
                NewQuery(DbSql.Member_DetectUsername,
                    Db.CreateParametersFromArray(
                        new object[,]
                        {
                            {"@Username", username}
                        })));
            return obj == null;
        }

        public bool DetectNickName(string nickname)
        {
            var obj = ExecuteScalar(
                NewQuery(DbSql.Member_DetectNickname,
                    Db.CreateParametersFromArray(
                        new object[,]
                        {
                            {"@Nickname", nickname}
                        })));
            return obj == null;
        }

        /// <summary>
        /// 检测用户名和昵称是否存在
        /// </summary>
        /// <param name="username"></param>
        /// <param name="nickname"></param>
        /// <returns></returns>
        public bool DetectUserAndNickNameExist(string username, string nickname)
        {
            var obj = ExecuteScalar(
                NewQuery(DbSql.Member_DetectUsernameAndNickNameHasExits,
                    Db.CreateParametersFromArray(
                        new object[,]
                        {
                            {"@Username", username},
                            {"@Nickname", nickname}
                        })));
            return obj != null;
        }

        /// <summary>
        /// 注册用户
        /// </summary>
        /// <param name="member"></param>
        public void RegisterMember(string username, string password, string avatar, string sex, string nickname,
            string note, string email, string telephone)
        {
            ExecuteNonQuery(
                NewQuery(DbSql.Member_RegisterMember,
                    Db.CreateParametersFromArray(
                        new object[,]
                        {
                            {"@Username", username},
                            {"@Password", password},
                            {"@Avatar", ""},
                            {"@Sex", sex},
                            {"@Nickname", nickname},
                            {"@Note", note},
                            {"@Email", email},
                            {"@Telephone", telephone}
                        })));
        }

        public void InsertDetails(int memberID, string status, string regIP, string token)
        {
            ExecuteNonQuery(
                NewQuery(DbSql.Member_InsertMemberDetails,
                    Db.CreateParametersFromArray(
                        new object[,]
                        {
                            {"@UId", memberID},
                            {"@Status", status},
                            {"@RegIP", regIP},
                            {"@RegTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")},
                            {"@LastLoginTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")},
                            {"@Token", token}
                        })));
        }

        /// <summary>
        /// 获取会员的UID
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public int GetMemberUid(string username)
        {
            var obj = ExecuteScalar(
                NewQuery(DbSql.Member_GetMemberUID,
                    Db.CreateParametersFromArray(
                        new object[,]
                        {
                            {"@Username", username}
                        }))
            );
            if (obj == null) return 0;
            return int.Parse(obj.ToString());
        }

        /// <summary>
        /// 验证用户并返回
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public void VerifyMember(string username, string password, DataReaderFunc func)
        {
            ExecuteReader(
                NewQuery(DbSql.Member_VerifyMember,
                    Db.CreateParametersFromArray(
                        new object[,]
                        {
                            {"@Username", username},
                            {"@Password", password}
                        })),
                func
            );
        }

        public void GetMemberByID(int id, DataReaderFunc func)
        {
            ExecuteReader(
                NewQuery(DbSql.Member_GetMemberByID,
                    Db.CreateParametersFromArray(
                        new object[,]
                        {
                            {"@id", id}
                        })),
                func
            );
        }

        public void GetMemberByUsername(string username, DataReaderFunc func)
        {
            ExecuteReader(
                NewQuery(DbSql.Member_GetMemberByUsername,
                    Db.CreateParametersFromArray(
                        new object[,]
                        {
                            {"@Username", username}
                        })),
                func
            );
        }


        /// <summary>
        /// 根据用户名获取会员详细信息
        /// </summary>
        /// <param name="username"></param>
        /// <param name="member"></param>
        /// <param name="details"></param>
        public void GetMemberDetailsByUsername(string username, DataReaderFunc func)
        {
            ExecuteReader(
                NewQuery(DbSql.Member_GetMemberInfoAndDetails,
                    Db.CreateParametersFromArray(
                        new object[,]
                        {
                            {"@id", -1},
                            {"@Username", username}
                        })),
                func
            );
        }

        /// <summary>
        /// 根据用户ID获取会员详细信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="member"></param>
        /// <param name="details"></param>
        public void GetMemberDetailsByID(int id, DataReaderFunc func)
        {
            ExecuteReader(
                NewQuery(DbSql.Member_GetMemberInfoAndDetails,
                    Db.CreateParametersFromArray(
                        new object[,]
                        {
                            {"@id", id},
                            {"@Username", string.Empty}
                        })),
                func
            );
        }

        /// <summary>
        /// 更新会员资料
        /// </summary>
        /// <param name="memberId"></param>
        /// <param name="password"></param>
        /// <param name="nickName"></param>
        /// <param name="avatar"></param>
        /// <param name="sex"></param>
        /// <param name="email"></param>
        /// <param name="telephone"></param>
        /// <param name="note"></param>
        public void Update(int memberId, string password, string nickName,
            string avatar, string sex, string email, string telephone, string note)
        {
            ExecuteNonQuery(
                NewQuery(DbSql.Member_Update,
                    Db.CreateParametersFromArray(
                        new object[,]
                        {
                            {"@Password", password},
                            {"@Avatar", avatar},
                            {"@Sex", sex},
                            {"@Nickname", nickName},
                            {"@Email", email},
                            {"@Telephone", telephone},
                            {"@Note", note},
                            {"@id", memberId}
                        })));
        }

        /// <summary>
        /// 删除会员
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int memberID)
        {
            var pa = new object[,] { { "@id", memberID } };
            var parameters = Db.CreateParametersFromArray(pa);
            ExecuteMultiNonQuery(new[]{NewQuery(DbSql.Member_DeleteMember, parameters),
                NewQuery(DbSql.Member_DeleteMemberDetails, parameters)});
        }

        /// <summary>
        /// 获取会员列表
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="currentPageIndex"></param>
        /// <param name="recordCount"></param>
        /// <param name="pageCount"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public DataTable GetPagedMembers(int pageSize, ref int currentPageIndex, out int recordCount, out int pages,
            DateTime? time)
        {
            //ACCESS时候第一页执行
            const string sql1 =
                @"SELECT TOP $[pagesize] [id],[username],[avatar],[nickname],[RegIp],[RegTime],[LastLoginTime] FROM $PREFIX_member INNER JOIN $PREFIX_MemberDetails ON $PREFIX_member.[ID]=$PREFIX_MemberDetails.[UID]";

            recordCount = int.Parse(
                ExecuteScalar(NewQuery(DbSql.Member_GetMemberCount, null)).ToString()
            );

            pages = recordCount / pageSize;
            if (recordCount % pageSize != 0) pages++;
            //验证当前页数

            if (currentPageIndex > pages && currentPageIndex != 1) currentPageIndex = pages;
            if (currentPageIndex < 1) currentPageIndex = 1;
            //计算分页
            var skipCount = pageSize * (currentPageIndex - 1);


            //如果调过记录为0条，且为OLEDB时候，则用sql1
            var sql = skipCount == 0 && DbType == DataBaseType.OLEDB
                ? OptimizeSql(sql1)
                : OptimizeSql(DbSql.Member_GetPagedMembers);


            sql = SQLRegex.Replace(sql, match =>
            {
                switch (match.Groups[1].Value)
                {
                    case "pagesize": return pageSize.ToString();
                    case "skipsize": return skipCount.ToString();
                }

                return null;
            });

            return GetDataSet(
                new SqlQuery(sql)
            ).Tables[0];
        }
    }
}