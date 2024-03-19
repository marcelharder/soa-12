using System.Xml.Linq;
using System.Collections.Generic;
using System.IO;
using System;
using Microsoft.AspNetCore.Hosting;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using api.Entities;
using api.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Net.Http;
using System.Text.Json;
using System.Text;

namespace api.Data
{
    public class OperatieDrops
    {
        XElement _testje;
        XElement _val;
        List<Class_Item> _help = new List<Class_Item>();
        private IWebHostEnvironment _env;
        private readonly IOptions<ComSettings> _com;
        private UserManager<AppUser> _userManager;
        private DataContext _context;

        private SpecialMaps _sp;

        public OperatieDrops(IWebHostEnvironment env,
        DataContext context,
        SpecialMaps sp,
        UserManager<AppUser> userManager,
        IOptions<ComSettings> com)
        {
            _env = env;
            _userManager = userManager;
            var content = _env.ContentRootPath;
            var filename = "conf/language_file.xml";
            var test = Path.Combine(content, filename);
            XElement testje = XElement.Load($"{test}");
            _testje = testje;
            _com = com;
            _sp = sp;

            _context = context;

            Class_Item _result = new Class_Item();
            _result.description = "Choose";
            _result.value = 0;
            _help.Add(_result);

        }

        #region <!--postop-->

        public async Task<List<Class_Item>> getAutologousBloodTiming()
        {
            await Task.Run(() =>
            {
                IEnumerable<XElement> op = _testje.Descendants("postop").Elements("autologous_blood_timing").Elements("items");
                foreach (XElement s in op)
                {
                    Class_Item _result = new Class_Item();
                    _result.description = s.Element("description").Value;
                    _result.value = Convert.ToInt32(s.Element("value").Value);
                    _help.Add(_result);
                }
            });
            return _help;
        }

        #endregion
        #region <!--ltx-->

        public async Task<List<Class_Item>> getLTXIndication()
        {
            await Task.Run(() =>
            {
                IEnumerable<XElement> op = _testje.Descendants("ltx").Elements("drp_1").Elements("items");
                foreach (XElement s in op)
                {
                    Class_Item _result = new Class_Item();
                    _result.description = s.Element("description").Value;
                    _result.value = Convert.ToInt32(s.Element("value").Value);
                    _help.Add(_result);
                }
            });
            return _help;
        }
        public async Task<List<Class_Item>> getLTXType()
        {
            await Task.Run(() =>
            {
                IEnumerable<XElement> op = _testje.Descendants("ltx").Elements("drp_2").Elements("items");
                foreach (XElement s in op)
                {
                    Class_Item _result = new Class_Item();
                    _result.description = s.Element("description").Value;
                    _result.value = Convert.ToInt32(s.Element("value").Value);
                    _help.Add(_result);
                }
            });
            return _help;
        }

        #endregion

        #region <!--operatie-->
        public List<Class_Item> getTimingOptions()
        {
            IEnumerable<XElement> op = _testje.Descendants("operatie").Elements("operatie_timing").Elements("items");
            foreach (XElement s in op)
            {
                Class_Item _result = new Class_Item();
                _result.description = s.Element("description").Value;
                _result.value = Convert.ToInt32(s.Element("value").Value);
                _help.Add(_result);
            }
            return _help;
        }
        public List<Class_Item> getUrgentOptions()
        {
            IEnumerable<XElement> op = _testje.Descendants("operatie").Elements("urgent_reasons").Elements("items");
            foreach (XElement s in op)
            {
                Class_Item _result = new Class_Item();
                _result.description = s.Element("description").Value;
                _result.value = Convert.ToInt32(s.Element("value").Value);
                _help.Add(_result);
            }
            return _help;
        }
        public List<Class_Item> getEmergentOptions()
        {
            IEnumerable<XElement> op = _testje.Descendants("operatie").Elements("emergent_reasons").Elements("items");
            foreach (XElement s in op)
            {
                Class_Item _result = new Class_Item();
                _result.description = s.Element("description").Value;
                _result.value = Convert.ToInt32(s.Element("value").Value);
                _help.Add(_result);
            }
            return _help;
        }
        public List<Class_Item> getInotropeOptions()
        {
            IEnumerable<XElement> op = _testje.Descendants("operatie").Elements("inotropica").Elements("items");
            foreach (XElement s in op)
            {
                Class_Item _result = new Class_Item();
                _result.description = s.Element("description").Value;
                _result.value = Convert.ToInt32(s.Element("value").Value);
                _help.Add(_result);
            }
            return _help;
        }
        public List<Class_Item> getPericardOptions()
        {
            IEnumerable<XElement> op = _testje.Descendants("operatie").Elements("pericard").Elements("items");
            foreach (XElement s in op)
            {
                Class_Item _result = new Class_Item();
                _result.description = s.Element("description").Value;
                _result.value = Convert.ToInt32(s.Element("value").Value);
                _help.Add(_result);
            }
            return _help;
        }
        public List<Class_Item> getPleuraOptions()
        {
            IEnumerable<XElement> op = _testje.Descendants("operatie").Elements("pleura").Elements("items");
            foreach (XElement s in op)
            {
                Class_Item _result = new Class_Item();
                _result.description = s.Element("description").Value;
                _result.value = Convert.ToInt32(s.Element("value").Value);
                _help.Add(_result);
            }
            return _help;
        }
        public List<Class_Item> getPacemakerOptions()
        {
            IEnumerable<XElement> op = _testje.Descendants("operatie").Elements("pacemaker").Elements("items");
            foreach (XElement s in op)
            {
                Class_Item _result = new Class_Item();
                _result.description = s.Element("description").Value;
                _result.value = Convert.ToInt32(s.Element("value").Value);
                _help.Add(_result);
            }
            return _help;
        }
        public List<Class_Item> getArray(int id)
        {
            IEnumerable<XElement> op = _testje.Descendants("operatie").Elements("category").Elements("items");
            foreach (XElement s in op)
            {
                if (s.Element("cat").Value == id.ToString())
                {
                    Class_Item _result = new Class_Item();
                    _result.description = s.Element("description").Value;
                    _result.value = Convert.ToInt32(s.Element("value").Value);
                    _help.Add(_result);
                }
            }
            _help.RemoveAt(0); // remove the Choose item for this list
            return _help;
        }

        #endregion

        #region <!--cpb-->
        public async Task<List<Class_Item>> getTypeCardiopleg()
        {
            await Task.Run(() =>
            {
                IEnumerable<XElement> op = _testje.Descendants("cpb").Elements("typeCardiopleg").Elements("items");
                foreach (XElement s in op)
                {
                    Class_Item _result = new Class_Item();
                    _result.description = s.Element("description").Value;
                    _result.value = Convert.ToInt32(s.Element("value").Value);
                    _help.Add(_result);
                }
            });
            return _help;
        }
        public async Task<List<Class_Item>> getMPT()
        {
            await Task.Run(() =>
                   {
                       IEnumerable<XElement> op = _testje.Descendants("cpb").Elements("myocardial_protection").Elements("items");
                       _help = getCABGDrops(op);
                   });
            return _help;
        }
        public async Task<List<Class_Item>> getCPB_art_choice()
        {
            await Task.Run(() =>
                   {
                       IEnumerable<XElement> op = _testje.Descendants("cpb").Elements("art_choice").Elements("items");
                       _help = getCABGDrops(op);
                   });
            return _help;
        }
        public async Task<List<Class_Item>> getCPB_ven_choice()
        {
            await Task.Run(() =>
                   {
                       IEnumerable<XElement> op = _testje.Descendants("cpb").Elements("ven_choice").Elements("items");
                       _help = getCABGDrops(op);
                   });
            return _help;
        }
        public async Task<List<Class_Item>> getCPB_delivery()
        {
            await Task.Run(() =>
                   {
                       IEnumerable<XElement> op = _testje.Descendants("cpb").Elements("delivery").Elements("items");
                       _help = getCABGDrops(op);
                   });
            return _help;
        }
        public async Task<List<Class_Item>> getCPB_iabp_ind()
        {
            await Task.Run(() =>
            {
                IEnumerable<XElement> op = _testje.Descendants("cpb").Elements("iabp_ind").Elements("items");
                _help = getCABGDrops(op);
            });
            return _help;
        }
        public async Task<List<Class_Item>> getCPB_iabp_timing()
        {
            await Task.Run(() =>
            {
                IEnumerable<XElement> op = _testje.Descendants("cpb").Elements("iabp_ind_when").Elements("items");
                _help = getCABGDrops(op);
            });
            return _help;
        }
        public async Task<List<Class_Item>> getCPB_nccp()
        {
            await Task.Run(() =>
            {
                IEnumerable<XElement> op = _testje.Descendants("cpb").Elements("nccp").Elements("items");
                _help = getCABGDrops(op);
            });
            return _help;
        }
        public async Task<List<Class_Item>> getCPB_aox()
        {
            await Task.Run(() =>
            {
                IEnumerable<XElement> op = _testje.Descendants("cpb").Elements("aox").Elements("items");
                _help = getCABGDrops(op);
            });
            return _help;
        }
        public async Task<List<Class_Item>> getCPB_timing()
        {
            await Task.Run(() =>
            {
                IEnumerable<XElement> op = _testje.Descendants("cpb").Elements("timing").Elements("items");
                _help = getCABGDrops(op);
            });
            return _help;
        }
        public async Task<List<Class_Item>> getCPB_temp()
        {
            await Task.Run(() =>
            {
                IEnumerable<XElement> op = _testje.Descendants("cpb").Elements("temp").Elements("items");
                _help = getCABGDrops(op);
            });
            return _help;
        }
        #endregion

        #region <!--Euroscore-->
        public async Task<List<Class_Item>> getWeightIntervention()
        {
            await Task.Run(() =>
                   {
                       IEnumerable<XElement> op = _testje.Descendants("history").Elements("weight_of_intervention").Elements("items");
                       _help = getCABGDrops(op);
                   });
            return _help;
        }
        public async Task<List<Class_Item>> getLVFunction()
        {
            await Task.Run(() =>
                   {
                       IEnumerable<XElement> op = _testje.Descendants("history").Elements("LV_function").Elements("items");
                       _help = getCABGDrops(op);
                   });
            return _help;
        }
        public async Task<List<Class_Item>> getPulmonaryHypertension()
        {
            await Task.Run(() =>
                   {
                       IEnumerable<XElement> op = _testje.Descendants("history").Elements("pulmonary_hypertension").Elements("items");
                       _help = getCABGDrops(op);
                   });
            return _help;
        }
        public async Task<List<Class_Item>> getGenderOptions()
        {
            await Task.Run(() =>
                  {
                      IEnumerable<XElement> op = _testje.Descendants("demographics").Elements("gender").Elements("items");
                      _help = getCABGDrops(op);
                  });
            return _help;
        }
        public async Task<List<Class_Item>> getAgeOptions()
        {
            await Task.Run(() =>
                  {
                      var t = new List<int>();
                      for (int i = 18; i < 90; i++)
                      {
                          t.Add(i);
                      }
                      _help = getGeneralDrops(t);
                  });
            return _help;
        }
        public async Task<List<Class_Item>> getCreatOptions()
        {
            await Task.Run(() =>
                  {
                      var t = new List<int>();
                      for (int i = 18; i < 400; i++)
                      {
                          t.Add(i);
                      }
                      _help = getGeneralDrops(t);
                  });
            return _help;
        }
        public async Task<List<Class_Item>> getWeightOptions()
        {
            await Task.Run(() =>
                  {
                      var t = new List<int>();
                      for (int i = 40; i < 180; i++)
                      {
                          t.Add(i);
                      }
                      _help = getGeneralDrops(t);
                  });
            return _help;
        }
        public async Task<List<Class_Item>> getHeightOptions()
        {
            await Task.Run(() =>
            {
                var t = new List<int>();
                for (int i = 120; i < 220; i++)
                {
                    t.Add(i);
                }
                _help = getGeneralDrops(t);
            });
            return _help;
        }
        public async Task<List<Class_Item>> getUrgency()
        {
            await Task.Run(() =>
                    {
                        IEnumerable<XElement> op = _testje.Descendants("operatie").Elements("operatie_timing").Elements("items");
                        _help = getCABGDrops(op);
                    });
            return _help;
        }
        public async Task<List<Class_Item>> getNYHA()
        {
            await Task.Run(() =>
                   {
                       IEnumerable<XElement> op = _testje.Descendants("history").Elements("select29").Elements("items");
                       _help = getCABGDrops(op);
                   });
            return _help;
        }
        public async Task<List<Class_Item>> getReasonUrgent()
        {
            await Task.Run(() =>
                   {
                       IEnumerable<XElement> op = _testje.Descendants("operatie").Elements("urgent_reasons").Elements("items");
                       _help = getCABGDrops(op);
                   });
            return _help;
        }
        public async Task<List<Class_Item>> getReasonEmergency()
        {
            await Task.Run(() =>
                   {
                       IEnumerable<XElement> op = _testje.Descendants("operatie").Elements("emergent_reasons").Elements("items");
                       _help = getCABGDrops(op);
                   });
            return _help;
        }
        #endregion

        #region <!--MinInv-->
        internal async Task<List<Class_Item>> getConversionDetails()
        {
            await Task.Run(() =>
            {
                IEnumerable<XElement> op = _testje.Descendants("min_inv").Elements("conversion").Elements("items");
                _help = getCABGDrops(op);
            });
            return _help;
        }
        internal async Task<List<Class_Item>> getStrategy()
        {
            await Task.Run(() =>
            {
                IEnumerable<XElement> op = _testje.Descendants("min_inv").Elements("strategy").Elements("items");
                _help = getCABGDrops(op);
            });
            return _help;
        }
        internal async Task<List<Class_Item>> getPrimaryIncision()
        {
            await Task.Run(() =>
            {
                IEnumerable<XElement> op = _testje.Descendants("min_inv").Elements("approach").Elements("items");
                _help = getCABGDrops(op);
            });
            return _help;
        }
        internal async Task<List<Class_Item>> getFollow_1()
        {
            await Task.Run(() =>
            {
                IEnumerable<XElement> op = _testje.Descendants("min_inv").Elements("follow_1").Elements("items");
                _help = getCABGDrops(op);
            });
            return _help;
        }
        internal async Task<List<Class_Item>> getFollow_2()
        {
            await Task.Run(() =>
            {
                IEnumerable<XElement> op = _testje.Descendants("min_inv").Elements("follow_2").Elements("items");
                _help = getCABGDrops(op);
            });
            return _help;
        }
        internal async Task<List<Class_Item>> getFollow_3()
        {
            await Task.Run(() =>
            {
                IEnumerable<XElement> op = _testje.Descendants("min_inv").Elements("follow_3").Elements("items");
                _help = getCABGDrops(op);
            });
            return _help;
        }
        internal async Task<List<Class_Item>> getLimaHarvest()
        {
            await Task.Run(() =>
            {
                IEnumerable<XElement> op = _testje.Descendants("min_inv").Elements("lima_harvest").Elements("items");
                _help = getCABGDrops(op);
            });
            return _help;
        }
        internal async Task<List<Class_Item>> getStabilization()
        {
            await Task.Run(() =>
            {
                IEnumerable<XElement> op = _testje.Descendants("min_inv").Elements("vessel").Elements("items");
                _help = getCABGDrops(op);
            });
            return _help;
        }
        #endregion

        #region <!--AorticSurgery-->
        public async Task<List<Class_Item>> getDissectionOnset()
        {
            await Task.Run(() =>
            {
                IEnumerable<XElement> op = _testje.Descendants("aortic_surgery").Elements("drp_78").Elements("items");
                _help = getCABGDrops(op);
            });
            return _help;
        }

        public async Task<List<Class_Item>> getDissectionType()
        {
            await Task.Run(() =>
            {
                IEnumerable<XElement> op = _testje.Descendants("aortic_surgery").Elements("drp_67").Elements("items");
                _help = getCABGDrops(op);
            });
            return _help;
        }

        public async Task<List<Class_Item>> getPathology()
        {
            await Task.Run(() =>
            {
                IEnumerable<XElement> op = _testje.Descendants("aortic_surgery").Elements("drp_1").Elements("items");
                _help = getCABGDrops(op);
            });
            return _help;
        }

        public async Task<List<Class_Item>> getOpIndication()
        {
            await Task.Run(() =>
            {
                IEnumerable<XElement> op = _testje.Descendants("aortic_surgery").Elements("drp_2").Elements("items");
                _help = getCABGDrops(op);
            });
            return _help;
        }

        internal async Task<List<Class_Item>> getSutureTechnique()
        {
            await Task.Run(() =>
            {
                IEnumerable<XElement> op = _testje.Descendants("min_inv").Elements("suture").Elements("items");
                _help = getCABGDrops(op);
            });
            return _help;
        }

        public async Task<List<Class_Item>> getAneurysmType()
        {
            await Task.Run(() =>
            {
                IEnumerable<XElement> op = _testje.Descendants("aortic_surgery").Elements("drp_54").Elements("items");
                _help = getCABGDrops(op);
            });
            return _help;
        }

        public async Task<List<Class_Item>> getOpTechnique()
        {
            await Task.Run(() =>
            {
                IEnumerable<XElement> op = _testje.Descendants("aortic_surgery").Elements("drp_3").Elements("items");
                _help = getCABGDrops(op);
            });
            return _help;
        }

        public async Task<List<Class_Item>> getRangeReplacement()
        {
            await Task.Run(() =>
            {
                IEnumerable<XElement> op = _testje.Descendants("aortic_surgery").Elements("drp_5").Elements("items");
                _help = getCABGDrops(op);
            });
            return _help;
        }
        #endregion

        #region <!--CABG--> 
        public async Task<List<Class_Item>> getCABGLocatie()
        {
            await Task.Run(() =>
                   {
                       IEnumerable<XElement> op = _testje.Descendants("cabg").Elements("locatie").Elements("items");
                       _help = getCABGDrops(op);
                   });
            return _help;
        }
        public async Task<List<Class_Item>> getCABGQuality()
        {
            await Task.Run(() =>
            {
                IEnumerable<XElement> op = _testje.Descendants("cabg").Elements("quality").Elements("items");
                _help = getCABGDrops(op);
            });
            return _help;
        }
        public async Task<List<Class_Item>> getCABGDiameter()
        {
            await Task.Run(() =>
            {
                IEnumerable<XElement> op = _testje.Descendants("cabg").Elements("diameter").Elements("items");
                _help = getCABGDrops(op);
            });
            return _help;
        }
        public async Task<List<Class_Item>> getCABGProximal()
        {
            await Task.Run(() =>
                   {
                       IEnumerable<XElement> op = _testje.Descendants("cabg").Elements("proximal").Elements("items");
                       _help = getCABGDrops(op);
                   });
            return _help;
        }

        public async Task<List<Class_Item>> getCABGConduit()
        {
            await Task.Run(() =>
                    {
                        IEnumerable<XElement> op = _testje.Descendants("cabg").Elements("conduit").Elements("items");
                        _help = getCABGDrops(op);
                    });
            return _help;
        }
        public async Task<List<Class_Item>> getCABGType()
        {
            await Task.Run(() =>
                    {
                        IEnumerable<XElement> op = _testje.Descendants("cabg").Elements("type").Elements("items");
                        _help = getCABGDrops(op);
                    });
            return _help;
        }
        public async Task<List<Class_Item>> getCABGProcedure()
        {
            await Task.Run(() =>
                    {
                        IEnumerable<XElement> op = _testje.Descendants("cabg").Elements("procedure").Elements("items");
                        _help = getCABGDrops(op);
                    });
            return _help;
        }
        public async Task<List<Class_Item>> getCABGAngle()
        {
            await Task.Run(() =>
                    {
                        IEnumerable<XElement> op = _testje.Descendants("cabg").Elements("angle").Elements("items");
                        _help = getCABGDrops(op);
                    });
            return _help;
        }
        public async Task<List<Class_Item>> getCABGDropList1()
        {
            await Task.Run(() =>
                            {
                                IEnumerable<XElement> op = _testje.Descendants("cabg").Elements("dropdownlist_1").Elements("items");
                                _help = getCABGDrops(op);
                            });
            return _help;
        }
        public async Task<List<Class_Item>> getCABGRadial()
        {
            await Task.Run(() =>
                            {
                                IEnumerable<XElement> op = _testje.Descendants("cabg").Elements("dropradial").Elements("items");
                                _help = getCABGDrops(op);
                            });
            return _help;
        }
        public async Task<List<Class_Item>> getCABGLeg()
        {
            await Task.Run(() =>
                            {
                                IEnumerable<XElement> op = _testje.Descendants("cabg").Elements("dropleg").Elements("items");
                                _help = getCABGDrops(op);
                            });
            return _help;
        }
        private List<Class_Item> getCABGDrops(IEnumerable<XElement> el)
        {
            foreach (XElement s in el)
            {
                Class_Item _result = new Class_Item();
                _result.description = s.Element("description").Value;
                _result.value = Convert.ToInt32(s.Element("value").Value);
                _help.Add(_result);
            }
            return _help;
        }
        #endregion

        #region <!--discharge--> 
        public async Task<List<Class_Item>> getDeadOrAlive()
        {
            await Task.Run(() =>
            {
                IEnumerable<XElement> op = _testje.Descendants("discharge").Elements("alive_dead").Elements("items");
                foreach (XElement s in op)
                {
                    Class_Item _result = new Class_Item();
                    _result.description = s.Element("description").Value;
                    _result.value = Convert.ToInt32(s.Element("value").Value);
                    _help.Add(_result);
                }
            });
            return _help;
        }
        public async Task<List<Class_Item>> getDeadLocation()
        {
            await Task.Run(() =>
            {
                IEnumerable<XElement> op = _testje.Descendants("discharge").Elements("location").Elements("items");
                foreach (XElement s in op)
                {
                    Class_Item _result = new Class_Item();
                    _result.description = s.Element("description").Value;
                    _result.value = Convert.ToInt32(s.Element("value").Value);
                    _help.Add(_result);
                }
            });
            return _help;
        }
        public async Task<List<Class_Item>> getDeadCause()
        {
            await Task.Run(() =>
            {
                IEnumerable<XElement> op = _testje.Descendants("discharge").Elements("cause").Elements("items");
                foreach (XElement s in op)
                {
                    Class_Item _result = new Class_Item();
                    _result.description = s.Element("description").Value;
                    _result.value = Convert.ToInt32(s.Element("value").Value);
                    _help.Add(_result);
                }
            });
            return _help;
        }
        public async Task<List<Class_Item>> getDischargeActivities()
        {
            await Task.Run(() =>
            {
                IEnumerable<XElement> op = _testje.Descendants("discharge").Elements("activities").Elements("items");
                foreach (XElement s in op)
                {
                    Class_Item _result = new Class_Item();
                    _result.description = s.Element("description").Value;
                    _result.value = Convert.ToInt32(s.Element("value").Value);
                    _help.Add(_result);
                }
            });
            return _help;
        }
        public async Task<List<Class_Item>> getDischargeDiagnosis()
        {
            await Task.Run(() =>
            {
                IEnumerable<XElement> op = _testje.Descendants("discharge").Elements("finaldiagnosis").Elements("items");
                foreach (XElement s in op)
                {
                    Class_Item _result = new Class_Item();
                    _result.description = s.Element("description").Value;
                    _result.value = Convert.ToInt32(s.Element("value").Value);
                    _help.Add(_result);
                }
            });
            return _help;
        }
        public async Task<List<Class_Item>> getDischargeDirection()
        {
            await Task.Run(() =>
            {
                IEnumerable<XElement> op = _testje.Descendants("discharge").Elements("discharged_to").Elements("items");
                foreach (XElement s in op)
                {
                    Class_Item _result = new Class_Item();
                    _result.description = s.Element("description").Value;
                    _result.value = Convert.ToInt32(s.Element("value").Value);
                    _help.Add(_result);
                }
            });
            return _help;
        }
        #endregion

        #region <!--Complicatie--> 
        public async Task<List<Class_Item>> getComplicatieOptie01()
        {
            await Task.Run(() =>
            {
                IEnumerable<XElement> op = _testje.Descendants("postop").Elements("complicatie_1").Elements("items");
                foreach (XElement s in op)
                {
                    Class_Item _result = new Class_Item();
                    _result.description = s.Element("description").Value;
                    _result.value = Convert.ToInt32(s.Element("value").Value);
                    _help.Add(_result);
                }
            });
            return _help;
        }
        public async Task<List<Class_Item>> getComplicatieOptie02()
        {
            await Task.Run(() =>
            {
                IEnumerable<XElement> op = _testje.Descendants("postop").Elements("complicatie_2").Elements("items");
                foreach (XElement s in op)
                {
                    Class_Item _result = new Class_Item();
                    _result.description = s.Element("description").Value;
                    _result.value = Convert.ToInt32(s.Element("value").Value);
                    _help.Add(_result);
                }
            });
            return _help;
        }
        public async Task<List<Class_Item>> getComplicatieOptie03()
        {
            await Task.Run(() =>
            {
                IEnumerable<XElement> op = _testje.Descendants("postop").Elements("complicatie_3").Elements("items");
                foreach (XElement s in op)
                {
                    Class_Item _result = new Class_Item();
                    _result.description = s.Element("description").Value;
                    _result.value = Convert.ToInt32(s.Element("value").Value);
                    _help.Add(_result);
                }
            });
            return _help;
        }
        public async Task<List<Class_Item>> getComplicatieOptie04()
        {
            await Task.Run(() =>
            {
                IEnumerable<XElement> op = _testje.Descendants("postop").Elements("complicatie_4").Elements("items");
                foreach (XElement s in op)
                {
                    Class_Item _result = new Class_Item();
                    _result.description = s.Element("description").Value;
                    _result.value = Convert.ToInt32(s.Element("value").Value);
                    _help.Add(_result);
                }
            });
            return _help;
        }
        public async Task<List<Class_Item>> getComplicatieOptie05()
        {
            await Task.Run(() =>
            {
                IEnumerable<XElement> op = _testje.Descendants("postop").Elements("complicatie_5").Elements("items");
                foreach (XElement s in op)
                {
                    Class_Item _result = new Class_Item();
                    _result.description = s.Element("description").Value;
                    _result.value = Convert.ToInt32(s.Element("value").Value);
                    _help.Add(_result);
                }
            });
            return _help;
        }
        public async Task<List<Class_Item>> getComplicatieOptie06()
        {
            await Task.Run(() =>
            {
                IEnumerable<XElement> op = _testje.Descendants("postop").Elements("complicatie_6").Elements("items");
                foreach (XElement s in op)
                {
                    Class_Item _result = new Class_Item();
                    _result.description = s.Element("description").Value;
                    _result.value = Convert.ToInt32(s.Element("value").Value);
                    _help.Add(_result);
                }
            });
            return _help;
        }
        public async Task<List<Class_Item>> getComplicatieOptie07()
        {
            await Task.Run(() =>
            {
                IEnumerable<XElement> op = _testje.Descendants("postop").Elements("complicatie_7").Elements("items");
                foreach (XElement s in op)
                {
                    Class_Item _result = new Class_Item();
                    _result.description = s.Element("description").Value;
                    _result.value = Convert.ToInt32(s.Element("value").Value);
                    _help.Add(_result);
                }
            });
            return _help;
        }
        public async Task<List<Class_Item>> getComplicatieOptie08()
        {
            await Task.Run(() =>
            {
                IEnumerable<XElement> op = _testje.Descendants("postop").Elements("complicatie_8").Elements("items");
                foreach (XElement s in op)
                {
                    Class_Item _result = new Class_Item();
                    _result.description = s.Element("description").Value;
                    _result.value = Convert.ToInt32(s.Element("value").Value);
                    _help.Add(_result);
                }
            });
            return _help;
        }
        public async Task<List<Class_Item>> getComplicatieOptie09()
        {
            await Task.Run(() =>
            {
                IEnumerable<XElement> op = _testje.Descendants("postop").Elements("complicatie_9").Elements("items");
                foreach (XElement s in op)
                {
                    Class_Item _result = new Class_Item();
                    _result.description = s.Element("description").Value;
                    _result.value = Convert.ToInt32(s.Element("value").Value);
                    _help.Add(_result);
                }
            });
            return _help;
        }
        #endregion

        #region <!--Valve--> 
        internal List<Class_Item> getTricuspidRingType()
        {
            IEnumerable<XElement> op = _val.Descendants("valve_codes");
            foreach (XElement s in op)
            {
                if (s.Element("fd_Type").Value == "R")
                {
                    Class_Item _result = new Class_Item();
                    _result.description = s.Element("Description").Value;
                    _result.value = Convert.ToInt32(s.Element("Valve_id").Value);
                    _help.Add(_result);
                }
            }


            return _help;
        }
        internal List<Class_Item> getMitralRingType()
        {
            IEnumerable<XElement> op = _val.Descendants("valve_codes");
            foreach (XElement s in op)
            {
                if (s.Element("fd_Type").Value == "R")
                {
                    Class_Item _result = new Class_Item();
                    _result.description = s.Element("Description").Value;
                    _result.value = Convert.ToInt32(s.Element("Valve_id").Value);
                    _help.Add(_result);
                }
            }


            return _help;
        }
        internal async Task<List<Class_Item>> getImplantPositionAsync()
        {
            await Task.Run(() =>
            {
                IEnumerable<XElement> op = _testje.Descendants("implantPosition").Elements("items");
                foreach (XElement s in op)
                {

                    Class_Item _result = new Class_Item();
                    _result.description = s.Element("description").Value;
                    _result.value = Convert.ToInt32(s.Element("value").Value);
                    _help.Add(_result);

                };
            });

            return _help;
        }
        public async Task<List<Class_Item>> getValveAetiology()
        {
            await Task.Run(() =>
            {
                IEnumerable<XElement> op = _testje.Descendants("valve").Elements("aetiology").Elements("items");
                foreach (XElement s in op)
                {
                    Class_Item _result = new Class_Item();
                    _result.description = s.Element("description").Value;
                    _result.value = Convert.ToInt32(s.Element("value").Value);
                    _help.Add(_result);
                }
            });
            return _help;
        }
        public async Task<List<Class_Item>> getAorticProcedure()
        {
            await Task.Run(() =>
            {
                IEnumerable<XElement> op = _testje.Descendants("valve").Elements("a_procedure").Elements("items");
                foreach (XElement s in op)
                {
                    Class_Item _result = new Class_Item();
                    _result.description = s.Element("description").Value;
                    _result.value = Convert.ToInt32(s.Element("value").Value);
                    _help.Add(_result);
                }
            });
            return _help;
        }
        public async Task<List<Class_Item>> getMitralProcedure()
        {
            await Task.Run(() =>
            {
                IEnumerable<XElement> op = _testje.Descendants("valve").Elements("m_procedure").Elements("items");
                foreach (XElement s in op)
                {
                    Class_Item _result = new Class_Item();
                    _result.description = s.Element("description").Value;
                    _result.value = Convert.ToInt32(s.Element("value").Value);
                    _help.Add(_result);
                }
            });
            return _help;
        }
        public async Task<List<Class_Item>> getTricuspidProcedure()
        {
            await Task.Run(() =>
            {
                IEnumerable<XElement> op = _testje.Descendants("valve").Elements("t_procedure").Elements("items");
                foreach (XElement s in op)
                {
                    Class_Item _result = new Class_Item();
                    _result.description = s.Element("description").Value;
                    _result.value = Convert.ToInt32(s.Element("value").Value);
                    _help.Add(_result);
                }
            });
            return _help;
        }
        public async Task<List<Class_Item>> getPulmonaryProcedure()
        {
            await Task.Run(() =>
            {
                IEnumerable<XElement> op = _testje.Descendants("valve").Elements("p_procedure").Elements("items");
                foreach (XElement s in op)
                {
                    Class_Item _result = new Class_Item();
                    _result.description = s.Element("description").Value;
                    _result.value = Convert.ToInt32(s.Element("value").Value);
                    _help.Add(_result);
                }
            });
            return _help;
        }
        public async Task<List<Class_Item>> getValveType()
        {
            await Task.Run(() =>
            {
                IEnumerable<XElement> op = _testje.Descendants("valve").Elements("valve_type").Elements("items");
                foreach (XElement s in op)
                {
                    Class_Item _result = new Class_Item();
                    _result.description = s.Element("description").Value;
                    _result.value = Convert.ToInt32(s.Element("value").Value);
                    _help.Add(_result);
                }
            });
            return _help;
        }
        public async Task<List<Class_Item>> getMitralValveRepair()
        {
            await Task.Run(() =>
            {
                IEnumerable<XElement> op = _testje.Descendants("valve").Elements("repair_type").Elements("mitral").Elements("items");
                foreach (XElement s in op)
                {
                    Class_Item _result = new Class_Item();
                    _result.description = s.Element("description").Value;
                    _result.value = Convert.ToInt32(s.Element("value").Value);
                    _help.Add(_result);
                }
            });
            return _help;
        }
        public async Task<List<Class_Item>> getTricuspidValveRepair()
        {
            await Task.Run(() =>
            {
                IEnumerable<XElement> op = _testje.Descendants("valve").Elements("repair_type").Elements("tricuspid").Elements("items");
                foreach (XElement s in op)
                {
                    Class_Item _result = new Class_Item();
                    _result.description = s.Element("description").Value;
                    _result.value = Convert.ToInt32(s.Element("value").Value);
                    _help.Add(_result);
                }
            });
            return _help;
        }
        #endregion

        private List<Class_Item> getGeneralDrops(List<int> list)
        {
            foreach (int h in list)
            {
                Class_Item _result = new Class_Item();
                _result.description = h.ToString();
                _result.value = h;
                _help.Add(_result);
            }
            return _help;
        }
        public async Task<List<Class_Item>> getYN()
        {
            await Task.Run(() =>
                            {
                                IEnumerable<XElement> op = _testje.Descendants("history").Elements("select1").Elements("items");
                                _help = getCABGDrops(op);
                            });
            return _help;
        }

        #region <!--HospitalStuff -->


        public async Task<List<Class_Item>> getHospitalOptions(int userId)
        {
            var cl = new List<Class_Item>();
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == userId);
            if (user != null)
            {
                var comaddress = _com.Value.hospitalURL;
                var st = "Hospital/getHospitalsWhereUserWorked/" + user.worked_in;
                comaddress = comaddress + st;
                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.GetAsync(comaddress))
                    {
                        var test = await response.Content.ReadAsStringAsync();
                        //cl = JsonSerializer.Deserialize<List<Class_Item>>(test);
                    }
                }
                return cl;

            }
            return null;
            }

        public async Task<List<Class_Item>> getAvailableHospitalOptions(string country)
        {
            var cl = new List<Class_Item>();
            var comaddress = _com.Value.hospitalURL;
            var st = "Hospital/getHospitalItemsPerCountryFromIso/" + country;
            comaddress = comaddress + st;
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(comaddress))
                {
                    var test = await response.Content.ReadAsStringAsync();
                    cl = JsonSerializer.Deserialize<List<Class_Item>>(test);
                }
            }
            return cl;
        }
        #endregion

        private List<Class_Item> compareList(List<Class_Item> _listA, List<Class_Item> _listB)
        {
            var result = new List<Class_Item>();
            foreach (var itemB in _listB)
            {
                var flat = 0;
                foreach (var itemA in _listA)
                {
                    if (itemA.value == itemB.value)
                    {
                        flat = 1;
                        break;
                    }
                }
                if (flat == 0)
                {
                    result.Add(itemB);
                }
            }
            return result;
        }

        public async Task<List<Class_Item>> getCareerItems()
        {
            await Task.Run(() =>
                            {
                                IEnumerable<XElement> op = _testje.Descendants("career").Elements("items");
                                _help = getCABGDrops(op);
                            });
            return _help;
        }

    }
}