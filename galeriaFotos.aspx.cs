
//using System;
//using System.Collections.Generic;
//using System.Web.UI;

//namespace proyectoPracticaProfecional
//{
//    public partial class galeriaFotos : System.Web.UI.Page
//    {
//        public class GalleryImage
//        {
//            public string Path { get; set; }
//            public string Title { get; set; }
//            public string Description { get; set; }
//        }

//        protected void Page_Load(object sender, EventArgs e)
//        {
//            if (!IsPostBack)
//            {
//                LoadGalleryImages();
//            }
//        }

//        private void LoadGalleryImages()
//        {
//             MODIFICA ESTAS RUTAS CON TUS QR
//            List<GalleryImage> images = new List<GalleryImage>
//            {
//                new GalleryImage { 
//                    Path = "images/ABC-512.jpeg", 
//                    Title = "PORTAL ABC", 
//                    Description = "Contacto Consultas generales (conmutador): (0221) 429-7600" 
//                },
//                new GalleryImage { 
//                    Path = "images/sadmatanza-1024.jpeg", 
//                    Title = "sad1 matanza", 
//                    Description = "Mesa de Consultas Generales: mesadeconsultasad1@gmail.com " 
//                },
//                new GalleryImage { 
//                    Path = "~/images/qr3.png", 
//                    Title = "Código QR 3", 
//                    Description = "Escanee para descargar la aplicación" 
//                }
//                 Agrega más QRs hasta 10...
//            };

//            rptImages.DataSource = images;
//            rptImages.DataBind();

//            rptIndicators.DataSource = images;
//            rptIndicators.DataBind();

//            lblTotalSlides.Text = images.Count.ToString();
//        }
//    }
//}
using System;
using System.Collections.Generic;
using System.Web.UI;

namespace proyectoPracticaProfecional
{
    public partial class galeriaFotos : System.Web.UI.Page
    {
        public class GalleryImage
        {
            public string Path { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public string LinkUrl { get; set; } // NUEVA PROPIEDAD PARA EL LINK
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadGalleryImages();
            }
        }

        private void LoadGalleryImages()
        {
            // MODIFICA ESTOS DATOS CON TUS QR Y LINKS
            List<GalleryImage> images = new List<GalleryImage>
            {
                new GalleryImage { 
                    Path = "images/ABC-512.jpeg", 
                    Title = "PORTAL ABC", 
                    Description = "Contacto Consultas generales (conmutador): (0221) 429-7600" ,
                    LinkUrl = "https://abc.gob.ar/" 
                },
                new GalleryImage { 
                    Path = "images/sadmatanza-1024.png", 
                    Title = "sad1 matanza", 
                    Description = "Mesa de Consultas Generales: mesadeconsultasad1@gmail.com " ,
                    LinkUrl = "https://sadmatanza.com/nsad/secretaria1/sad1.aspx" 
                },
                new GalleryImage { 
                    Path = "images/sad2-512.png", 
                    Title = "SAD2 matanza", 
                    Description = "SAD 2 La Matanza - Pte Peron 2876 San Justo - Telefonos: 4651-8513 / 4484-2014",
                    LinkUrl = "https://www.sadmatanza.com/secretaria2/indexS2.html" 
                }
                // Agrega más QRs con sus links...
            };

            rptImages.DataSource = images;
            rptImages.DataBind();

            rptIndicators.DataSource = images;
            rptIndicators.DataBind();

            lblTotalSlides.Text = images.Count.ToString();
        }
    }
}