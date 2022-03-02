using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Plumbing;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitAPITraining_3_3
{
    [Transaction(TransactionMode.Manual)]
    public class Main : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;
            IList<Reference> selectedRefList = uidoc.Selection.PickObjects(ObjectType.Element, "Выберите трубу");
            var elementList = new List<Element>();
           
            foreach (var selectedElement in selectedRefList)
            {
                Element element = doc.GetElement(selectedElement);
                elementList.Add(element);
             
               
                if (element is Pipe)
                {
                    Parameter LengthParameter = element.get_Parameter(BuiltInParameter.CURVE_ELEM_LENGTH);

                    if (LengthParameter.StorageType == StorageType.Double)
                    {
                        double LValue = LengthParameter.AsDouble();
                        double LValue1 = 1.1 * LValue;
                       
                        using (Transaction ts = new Transaction(doc, "Set parameters"))
                        {
                            ts.Start();
                            var FamilyInstance = element as Pipe;
                            Parameter Lparameter = FamilyInstance.LookupParameter("Длина с запасом");
                            Lparameter.Set(LValue1);
                            ts.Commit();
                        }
                        
                       
                    }
                    
                }


            }
            TaskDialog.Show("Length", "Параметры длины с запасом заполнены");

            return Result.Succeeded;
        }
    }
    
}
