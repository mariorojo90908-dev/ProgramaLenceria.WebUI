# Sistema de Gestión Comercial y POS Móvil para Lencería

Sistema integral de gestión de catálogo, control de inventario por variantes (talle y color) y módulo de cobro rápido optimizado para mostrador y ferias.

---

## Arquitectura de Software (Clean Architecture - 4 Capas)

El proyecto está diseñado bajo principios de Clean Architecture, garantizando desacoplamiento, mantenibilidad y separación estricta de responsabilidades:

- **ProgramaLenceria.Domain:** Entidades centrales del negocio (`Producto`, `Variante`, `Venta`, `Cliente`, `Proveedor`) sin dependencias externas.
- **ProgramaLenceria.Application:** Interfaces de repositorios, contratos de servicios, DTOs y lógica de negocio (cálculo de costos por docena, márgenes de ganancia y reglas de stock).
- **ProgramaLenceria.Infrastructure:** Implementación del acceso a datos mediante Entity Framework Core, repositorios concretos y persistencia con SQLite.
- **ProgramaLenceria.WebUI:** Capa de presentación interactiva con Blazor Server, componentes reutilizables y servicios de interfaz de usuario.

---

## Características Funcionales

- **Control de Stock por Variantes:** Seguimiento individual por combinación de talle, color y producto.
- **Cálculo Automático de Precios:** Conversión de costo por docena a unitario con aplicación dinámica de margen de rentabilidad.
- **Buscador Predictivo Multitérmino:** Algoritmo de filtrado no secuencial en tiempo real para búsqueda rápida por atributos (ej: `"conjunto 90 algodón"`).
- **Punto de Venta Táctil (Modo Feria):** Interfaz optimizada para dispositivos móviles con selector rápido de medios de pago, promociones y generación de tickets.

---

## Stack Tecnológico

- **Plataforma:** .NET 8
- **UI:** Blazor Server (Interactive Server Components) + Bootstrap 5
- **ORM / Persistencia:** Entity Framework Core & SQLite
- **Patrones:** Repository Pattern, Dependency Injection, DTOs

---

## Puesta en Marcha Local

1. Clonar el repositorio:
   ```bash
   git clone [https://github.com/mariorojo90908-dev/ProgramaLenceria.WebUI.git](https://github.com/mariorojo90908-dev/ProgramaLenceria.WebUI.git)
