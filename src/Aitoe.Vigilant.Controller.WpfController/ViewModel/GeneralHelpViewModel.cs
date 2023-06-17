using Aitoe.Vigilant.Controller.WpfController.Infra;
using GalaSoft.MvvmLight;
using System;
using System.IO.Packaging;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Xps.Packaging;

namespace Aitoe.Vigilant.Controller.WpfController.ViewModel
{
    public class GeneralHelpViewModel : ViewModelBase, IPageViewModel
    {
        public string Name
        {
            get
            {
                return "_General Help";
            }
        }

        private string _exeConfigPathUserLevelNone;

        

        public string ExeConfigPathUserLevelNone
        {
            get
            {
                return _exeConfigPathUserLevelNone;
            }

            set
            {
                if (_exeConfigPathUserLevelNone == value)
                    return;
                
                _exeConfigPathUserLevelNone = value;
                RaisePropertyChanged(() => ExeConfigPathUserLevelNone);
            }
        }

        private string _exeConfigPathUserLevelPerUserRoaming;

        public string ExeConfigPathUserLevelPerUserRoaming
        {
            get { return _exeConfigPathUserLevelPerUserRoaming; }
            set
            {
                if (_exeConfigPathUserLevelPerUserRoaming == value)
                    return;

                _exeConfigPathUserLevelPerUserRoaming = value;
                RaisePropertyChanged(() => ExeConfigPathUserLevelPerUserRoaming);

            }
        }

        private string _exeConfigPathUserLevelPerUserRoamingAndLocal;
        public string ExeConfigPathUserLevelPerUserRoamingAndLocal
        {
            get { return _exeConfigPathUserLevelPerUserRoamingAndLocal; }
            set
            {
                if (_exeConfigPathUserLevelPerUserRoamingAndLocal == value)
                    return;
                
                _exeConfigPathUserLevelPerUserRoamingAndLocal = value;
                RaisePropertyChanged(() => ExeConfigPathUserLevelPerUserRoamingAndLocal);
            }
        }

        private Uri XpsDocumentUri { get; set; }
        //FixedDocumentSequence DocumentPath { get; set; }

        private FixedDocumentSequence docPath;

        public FixedDocumentSequence DocumentPath
        {
            get { return docPath; }
            set {
                docPath = value;
                RaisePropertyChanged();
            }
        }


        public GeneralHelpViewModel()
        {
            XpsDocumentUri = new Uri("pack://application:,,,/Resources/Docs/aiSentinelManual.xps");

            var stream = Application.GetResourceStream(XpsDocumentUri).Stream;
            Package package = Package.Open(stream);
            PackageStore.AddPackage(XpsDocumentUri, package);
            var xpsDoc = new XpsDocument(package, CompressionOption.Maximum, XpsDocumentUri.AbsoluteUri);
            DocumentPath = xpsDoc.GetFixedDocumentSequence();
            //_vw.Document = fixedDocumentSequence; // displaying document in viewer
            xpsDoc.Close();

            //http://www.c-sharpcorner.com/UploadFile/mahesh/viewing-word-documents-in-wpf/
            ExeConfigPathUserLevelNone = Utils.GetConfigLocation(1);
            ExeConfigPathUserLevelPerUserRoaming = Utils.GetConfigLocation(2);
            ExeConfigPathUserLevelPerUserRoaming = Utils.GetConfigLocation(3);
        }
    }
}
