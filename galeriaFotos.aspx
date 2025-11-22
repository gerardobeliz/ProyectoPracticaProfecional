<%@ Page Title="Galería de Fotos" Language="C#" MasterPageFile="~/Principal.Master" AutoEventWireup="true" CodeBehind="galeriaFotos.aspx.cs" Inherits="proyectoPracticaProfecional.galeriaFotos" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <!-- Bootstrap -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet">
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>

    <style>
        .gallery-modern {
            max-width: 1000px;
            margin: 0 auto;
            padding: 60px 20px;
        }
        .carousel-card {
            background: linear-gradient(135deg, #ffffff 0%, #f8f9fa 100%);
            border-radius: 24px;
            padding: 40px;
            box-shadow: 
                0 25px 50px -12px rgba(0, 0, 0, 0.15),
                0 0 0 1px rgba(0, 0, 0, 0.05);
            border: none;
        }
        .carousel-card .carousel-item {
            transition: none;
        }
        .qr-link {
            display: block;
            text-decoration: none;
            transition: all 0.3s ease;
            border-radius: 12px;
        }
        .qr-link:hover {
            transform: scale(1.02);
            box-shadow: 0 8px 25px rgba(0, 0, 0, 0.15);
        }
        .carousel-card .carousel-item img {
            width: 100%;
            height: auto;
            max-height: 400px;
            object-fit: contain;
            border-radius: 12px;
            margin-bottom: 30px;
            background: white;
            padding: 20px;
            box-shadow: 0 4px 15px rgba(0, 0, 0, 0.08);
            cursor: pointer;
        }
        .card-content {
            text-align: center;
            padding: 0 10px;
        }
        .card-title {
            font-size: 1.8rem;
            font-weight: 600;
            color: #1a1a1a;
            margin-bottom: 20px;
            line-height: 1.3;
            letter-spacing: -0.3px;
        }
        .card-description {
            font-size: 1.1rem;
            color: #4a5568;
            line-height: 1.7;
            font-weight: 400;
            max-width: 600px;
            margin: 0 auto;
        }
        .link-url {
            display: block;
            margin-top: 15px;
            font-size: 0.9rem;
            color: #007bff;
            word-break: break-all;
            text-decoration: none;
        }
        .link-url:hover {
            text-decoration: underline;
        }
        .carousel-card .carousel-control-prev,
        .carousel-card .carousel-control-next {
            width: 50px;
            height: 50px;
            background: rgba(255, 255, 255, 0.95);
            border-radius: 50%;
            top: 50%;
            transform: translateY(-50%);
            opacity: 1;
            transition: all 0.3s ease;
            box-shadow: 0 4px 20px rgba(0, 0, 0, 0.15);
            border: 1px solid rgba(0, 0, 0, 0.08);
        }
        .carousel-card .carousel-control-prev {
            left: -25px;
        }
        .carousel-card .carousel-control-next {
            right: -25px;
        }
        .carousel-card .carousel-control-prev-icon,
        .carousel-card .carousel-control-next-icon {
            filter: invert(0.4);
            width: 18px;
            height: 18px;
        }
        .carousel-card .carousel-indicators {
            bottom: -50px;
        }
        .carousel-card .carousel-indicators button {
            width: 10px;
            height: 10px;
            border-radius: 50%;
            margin: 0 6px;
            background-color: #cbd5e0;
            border: none;
            transition: all 0.3s ease;
        }
        .carousel-card .carousel-indicators button.active {
            background-color: #4a5568;
            transform: scale(1.2);
        }
        .carousel-card .carousel-indicators button:hover {
            background-color: #718096;
        }
        .gallery-title {
            text-align: center;
            font-weight: 350;
            font-size: 2.8rem;
            margin-bottom: 50px;
            color: #1a202c;
            letter-spacing: -0.8px;
        }
        .slide-counter {
            text-align: center;
            margin-top: 60px;
            color: #718096;
            font-size: 0.9rem;
            font-weight: 500;
            letter-spacing: 0.5px;
        }
        .qr-instructions {
            text-align: center;
            margin-top: 20px;
            color: #718096;
            font-size: 0.9rem;
            font-style: italic;
        }

        /* Responsive */
        @media (max-width: 768px) {
            .gallery-modern {
                padding: 40px 15px;
            }
            .carousel-card {
                padding: 25px 20px;
            }
            .carousel-card .carousel-item img {
                max-height: 300px;
                padding: 15px;
            }
            .card-title {
                font-size: 1.5rem;
            }
            .card-description {
                font-size: 1rem;
            }
            .carousel-card .carousel-control-prev,
            .carousel-card .carousel-control-next {
                width: 40px;
                height: 40px;
            }
            .carousel-card .carousel-control-prev {
                left: -15px;
            }
            .carousel-card .carousel-control-next {
                right: -15px;
            }
        }

        @media (max-width: 480px) {
            .carousel-card .carousel-item img {
                max-height: 250px;
                padding: 10px;
            }
        }
    </style>

    <div class="gallery-modern">
        <h2 class="gallery-title">TELEFONOS Y LINKS UTILES</h2>
        
        <div id="galleryCarousel" class="carousel slide carousel-card" data-bs-ride="false">
            
            <!-- Indicadores -->
            <div class="carousel-indicators">
                <asp:Repeater ID="rptIndicators" runat="server">
                    <ItemTemplate>
                        <button type="button" data-bs-target="#galleryCarousel" 
                                data-bs-slide-to="<%# Container.ItemIndex %>" 
                                class='<%# Container.ItemIndex == 0 ? "active" : "" %>'
                                aria-label='Slide <%# Container.ItemIndex + 1 %>'></button>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
            
            <!-- Contenido del Carrusel -->
            <div class="carousel-inner">
                <asp:Repeater ID="rptImages" runat="server">
                    <ItemTemplate>
                        <div class='carousel-item <%# Container.ItemIndex == 0 ? "active" : "" %>'>
                            <!-- Imagen/QR como LINK -->
                            <a href='<%# Eval("LinkUrl") %>' class="qr-link" target="_blank">
                                <img src='<%# Eval("Path") %>' 
                                     class="d-block w-100" 
                                     alt='<%# Eval("Title") %>'
                                     loading="lazy"
                                     title='Haga clic para abrir: <%# Eval("LinkUrl") %>'>
                            </a>
                            
                            <!-- Contenido de la tarjeta -->
                            <div class="card-content">
                                <h3 class="card-title"><%# Eval("Title") %></h3>
                                <p class="card-description"><%# Eval("Description") %></p>
                                <a href='<%# Eval("LinkUrl") %>' class="link-url" target="_blank">
                                    <%# Eval("LinkUrl") %>
                                </a>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
            
            <!-- Controles de navegación -->
            <button class="carousel-control-prev" type="button" data-bs-target="#galleryCarousel" data-bs-slide="prev">
                <span class="carousel-control-prev-icon" aria-hidden="true"></span>
                <span class="visually-hidden">Anterior</span>
            </button>
            <button class="carousel-control-next" type="button" data-bs-target="#galleryCarousel" data-bs-slide="next">
                <span class="carousel-control-next-icon" aria-hidden="true"></span>
                <span class="visually-hidden">Siguiente</span>
            </button>
        </div>

        <!-- Contador de slides -->
        <div class="slide-counter">
            <span id="currentSlide">1</span> / <asp:Label ID="lblTotalSlides" runat="server" Text=""></asp:Label>
        </div>

        <!-- Instrucciones para QR -->
        <div class="qr-instructions">
            Haga clic en cualquier código QR para abrir el enlace • Use los botones para navegar
        </div>
    </div>

    <!-- Script -->
    <script>
        document.addEventListener('DOMContentLoaded', function() {
            const carousel = document.getElementById('galleryCarousel');
            const currentSlide = document.getElementById('currentSlide');
            
            carousel.addEventListener('slid.bs.carousel', function (e) {
                const activeIndex = Array.from(e.relatedTarget.parentElement.children).indexOf(e.relatedTarget);
                currentSlide.textContent = activeIndex + 1;
            });
        });
    </script>

</asp:Content>