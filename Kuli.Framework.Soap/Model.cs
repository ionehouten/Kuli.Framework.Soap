using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Kuli.Framework.Soap
{
    public abstract class Model : IModel
    {
        public Type SelectClient;
        public Type SelectOutput;
        public Type SelectRow;
        public String SelectMember;
        public List<dynamic> SelectList = new List<dynamic>();
        public Type CrudClient;
        public Type CrudOutput;
        public Int32 SelectCount = 0;

        public Model()
        {

        }

        public IAsyncResult result = null;
        public virtual void AfterExecuteData(Object output, Exception ex =null) { }
        public virtual void AfterLoadData(Object output, Exception ex = null) { }
        public virtual void BeforeExecuteData() { }
        public virtual void BeforeLoadData() { }
        public virtual Object ExecuteData(dynamic input)
        {
            this.BeforeExecuteData();
            dynamic crudclient = Activator.CreateInstance(CrudClient);
            dynamic crudoutput = Activator.CreateInstance(CrudOutput);
            try
            {
                this.result = crudclient.GetType().GetMethod("Beginexecute").Invoke(crudclient, new Object[] { input, null, null }) as IAsyncResult;
                this.result.AsyncWaitHandle.WaitOne();
                this.result.AsyncWaitHandle.Close();
                crudoutput = crudclient.GetType().GetMethod("Endexecute").Invoke(crudclient, new Object[] { this.result });
                crudclient.GetType().GetMethod("Close").Invoke(crudclient, new Object[] { });

               
                this.AfterExecuteData(crudoutput);
                return crudoutput;
            }
            catch (Exception ex)
            {
                this.AfterExecuteData(crudoutput, ex);
                return null;
            }
        }

        public virtual Object ExecuteDataCreate(dynamic input)
        {
            return this.ExecuteData(input);
        }
        public virtual Object ExecuteDataUpdate(dynamic input)
        {
            return this.ExecuteData(input);
        }
        public virtual Object ExecuteDataDownload(dynamic input)
        {
            return this.ExecuteData(input);
        }
        public virtual async Task<dynamic> ExecuteDataTask(dynamic input)
        {
            return await Task.Run(() => ExecuteData(input));
            //Object obj = await Task.FromResult<Object>(ExecuteData(input));
            //return obj;
        }
        public virtual async Task<dynamic> ExecuteDataUpdateTask(dynamic input)
        {
            return await Task.Run(() => ExecuteDataUpdate(input));
            //Object obj = await Task.FromResult<Object>(ExecuteDataUpdate(input));
            //return obj;
        }
        public virtual async Task<dynamic> ExecuteDataCreateTask(dynamic input)
        {
            return await Task.Run(() => ExecuteDataCreate(input));
            //Object obj = await Task.FromResult<Object>(ExecuteDataCreate(input));
            //return obj;
        }
        public virtual async Task<dynamic> ExecuteDataDownloadTask(dynamic input)
        {
            return await Task.Run(() => ExecuteDataDownload(input));
            //Object obj = await Task.FromResult<Object>(ExecuteDataDownload(input));
            //return obj;
        }
        public virtual void LoadData(dynamic input, BindingSource source , Control control , Boolean more = false)
        {
            this.BeforeLoadData();
            dynamic selectclient = Activator.CreateInstance(SelectClient);
            dynamic selectoutput = Activator.CreateInstance(SelectOutput);

            try
            {
                this.result = selectclient.GetType().GetMethod("Beginexecute").Invoke(selectclient, new Object[] { input, null, null }) as IAsyncResult;
                this.result.AsyncWaitHandle.WaitOne();
                this.result.AsyncWaitHandle.Close();
                selectoutput = selectclient.GetType().GetMethod("Endexecute").Invoke(selectclient, new Object[] { this.result });
                selectclient.GetType().GetMethod("Close").Invoke(selectclient, new Object[] {});
                
                selectoutput = selectoutput.GetType().GetProperty(SelectMember).GetValue(selectoutput);

                if (control != null)
                {
                    control.Invoke(
                        new Action(() =>
                        {
                            const BindingFlags flags = BindingFlags.Public | BindingFlags.Instance;
                            List<dynamic> data = new List<dynamic>();
                            List<dynamic> dataConvert = new List<dynamic>();
                            data.AddRange(selectoutput);
                            this.SelectCount = data.Count();
                            if (data.Count() > 0)
                            {
                                int i = 0;
                                foreach (var item in data)
                                {
                                    Object row = item;
                                   
                                    var objFieldNames = row.GetType().GetProperties(flags).Cast<PropertyInfo>().
                                    Select(x => new
                                    {
                                        Name = x.Name,
                                        Type = Nullable.GetUnderlyingType(x.PropertyType) ?? x.PropertyType
                                    }).ToList();
                                    objFieldNames = objFieldNames.Where(x => x.Type == typeof(DateTime) || x.Type == typeof(DateTime?)).ToList();
                                    if (objFieldNames != null)
                                    {
                                        foreach (var itemdetail in objFieldNames)
                                        {
                                            PropertyInfo propertyInfos = row.GetType().GetProperty(itemdetail.Name);
                                            if (propertyInfos.PropertyType == typeof(DateTime) || propertyInfos.PropertyType == typeof(DateTime?))
                                            {
                                                propertyInfos.SetValue
                                                (row, Converter.CheckDate(Converter.toDateTime(propertyInfos.GetValue(row))), null);
                                            }
                                            else
                                            {
                                                propertyInfos.SetValue(row, propertyInfos.GetValue(row), null);
                                            }

                                        }
                                    }
                                    dataConvert.Add(row);
                                    i++;

                                }
                                data = dataConvert;
                                dataConvert = null;
                            }
                         
                            
                            if (more == false || source.DataSource == null)
                            {
                                SelectList = new List<dynamic>();
                                SelectList = data;
                                if (SelectList.Count > 0)
                                {
                                    source.DataSource = SelectRow;
                                    source.DataSource = SelectList;
                                }
                                else
                                {
                                    source.DataSource = SelectRow;
                                }
                               
                            }
                            else
                            {
                                SelectList.AddRange(data);
                            }
                            data = null;
                            
                            source.ResetBindings(false);
                            source.ResetCurrentItem();


                        }
                    ));
                }

                this.AfterLoadData(selectoutput);
                selectoutput = null;
            }
            catch (Exception ex)
            {
                source.DataSource = SelectRow;
                this.AfterLoadData(selectoutput,ex);
            }
            
        }
        public virtual dynamic LoadData(dynamic input)
        {
            this.BeforeLoadData();
            dynamic selectclient = Activator.CreateInstance(SelectClient);
            dynamic selectoutput = Activator.CreateInstance(SelectOutput);

            try
            {
                this.result = selectclient.GetType().GetMethod("Beginexecute").Invoke(selectclient, new Object[] { input, null, null }) as IAsyncResult;
                this.result.AsyncWaitHandle.WaitOne();
                this.result.AsyncWaitHandle.Close();
                selectoutput = selectclient.GetType().GetMethod("Endexecute").Invoke(selectclient, new Object[] { this.result });
                selectclient.GetType().GetMethod("Close").Invoke(selectclient, new Object[] { });

                selectoutput = selectoutput.GetType().GetProperty(SelectMember).GetValue(selectoutput);
                
                List<dynamic> listData = new List<dynamic>();
                listData.AddRange(selectoutput);
                this.AfterLoadData(listData);
                selectoutput = null;
                return listData;
            }
            catch (Exception ex)
            {
                this.AfterLoadData(selectoutput, ex);
                return null;
            }

        }
        public virtual async Task LoadDataTask(dynamic input, BindingSource source, Control control, Boolean more = false)
        {
            await Task.Run(() =>
            {
                LoadData(input, source, control, more);

            });
        }
        public virtual async Task<dynamic> LoadDataTask(dynamic input)
        {
            return await Task.Run(() => LoadData(input));
            //Object obj = await Task.FromResult<dynamic>(LoadData(input));
            //return obj;
        }
    }
}
