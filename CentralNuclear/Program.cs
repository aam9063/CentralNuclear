using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Xml.Serialization;

// Enumeración para los estados de los componentes
public enum EstadoComponente { Encendido, Apagado, Mantenimiento }

// Clase abstracta ComponenteNuclear
public abstract class ComponenteNuclear
{
    public string Nombre { get; set; }
    public EstadoComponente Estado { get; set; }

    protected ComponenteNuclear(string nombre, EstadoComponente estado)
    {
        Nombre = nombre;
        Estado = estado;
    }

    public override string ToString()
    {
        return $"Nombre: {Nombre}, Estado: {Estado}";
    }
}

// Clase Reactor que deriva de ComponenteNuclear
public class Reactor : ComponenteNuclear, IMantenible
{
    public double NivelRadiacion { get; set; }
    public double Temperatura { get; set; }

    public Reactor(string nombre, EstadoComponente estado, double nivelRadiacion) : base(nombre, estado)
    {
        NivelRadiacion = nivelRadiacion;
    }

    public void RealizarMantenimiento()
    {
        Estado = EstadoComponente.Mantenimiento;
        // Lógica para reducir el nivel de radiación
        NivelRadiacion -= 10;
    }

    public override string ToString()
    {
        return base.ToString() + $", Nivel de Radiación: {NivelRadiacion}";
    }
}

// Clase SistemaRefrigeracion que deriva de ComponenteNuclear
public class SistemaRefrigeracion : ComponenteNuclear
{
    public double Temperatura { get; set; }

    public SistemaRefrigeracion(string nombre, EstadoComponente estado, double temperatura) : base(nombre, estado)
    {
        Temperatura = temperatura;
    }

    public override string ToString()
    {
        return base.ToString() + $", Temperatura: {Temperatura}";
    }
}

// Interfaz IMantenible
public interface IMantenible
{
    void RealizarMantenimiento();
}

public class CentralNuclear
{
    private List<ComponenteNuclear> componentes;

    public CentralNuclear()
    {
        componentes = new List<ComponenteNuclear>();
    }

    public void AgregarComponente(ComponenteNuclear componente)
    {
        if (componente != null)
        {
            componentes.Add(componente);
            Console.WriteLine($"Componente {componente.Nombre} agregado a la central.");
        }
        else
        {
            Console.WriteLine("No se puede agregar un componente nulo.");
        }
    }

    public void RealizarMantenimientoGeneral()
    {
        componentes.OfType<IMantenible>().ToList().ForEach(mantenible =>
        {
            Console.WriteLine($"Realizando mantenimiento en el componente {((ComponenteNuclear)mantenible).Nombre}...");
            mantenible.RealizarMantenimiento();
            Console.WriteLine($"Mantenimiento completado en el componente {((ComponenteNuclear)mantenible).Nombre}.");
        });
    }



    public void MostrarEstadoCentral()
    {
        foreach (var componente in componentes)
        {
            Console.WriteLine(componente);
        }
    }

    public void EncenderReactor(string nombre)
    {
        var reactor = componentes.OfType<Reactor>().FirstOrDefault(r => r.Nombre == nombre);
        if (reactor != null)
        {
            reactor.Estado = EstadoComponente.Encendido;
            reactor.NivelRadiacion += 10; // Aumentar el nivel de radiación
            Console.WriteLine($"El reactor {nombre} ha sido encendido. Nivel de radiación: {reactor.NivelRadiacion}");
        }
        else
        {
            Console.WriteLine("No se encontró el reactor con el nombre proporcionado.");
        }
    }

    public void ApagarReactor(string nombre)
    {
        var reactor = componentes.OfType<Reactor>().FirstOrDefault(r => r.Nombre == nombre);
        if (reactor != null)
        {
            reactor.Estado = EstadoComponente.Apagado;
            reactor.NivelRadiacion -= 10; // Reducir el nivel de radiación
            Console.WriteLine($"El reactor {nombre} ha sido apagado. Nivel de radiación: {reactor.NivelRadiacion}");
        }
        else
        {
            Console.WriteLine("No se encontró el reactor con el nombre proporcionado.");
        }
    }


    public void RegularTemperaturaReactor(string nombre, double temperatura)
    {
        var sistemaRefrigeracion = componentes.OfType<SistemaRefrigeracion>().FirstOrDefault(sr => sr.Nombre == nombre);
        if (sistemaRefrigeracion != null)
        {
            sistemaRefrigeracion.Temperatura = temperatura;
            Console.WriteLine($"La temperatura del sistema de refrigeración {nombre} ha sido regulada a {temperatura} grados.");
        }
        else
        {
            Console.WriteLine("No se encontró el sistema de refrigeración con el nombre proporcionado.");
        }
    }



    public void EncenderSistemaRefrigeracion(string nombre)
    {
        var sistemaRefrigeracion = componentes.OfType<SistemaRefrigeracion>().FirstOrDefault(sr => sr.Nombre == nombre);
        if (sistemaRefrigeracion != null)
        {
            sistemaRefrigeracion.Estado = EstadoComponente.Encendido;
            Console.WriteLine($"El sistema de refrigeración {nombre} ha sido encendido.");
        }
        else
        {
            Console.WriteLine("No se encontró el sistema de refrigeración con el nombre proporcionado.");
        }
    }

    public void ApagarSistemaRefrigeracion(string nombre)
    {
        var sistemaRefrigeracion = componentes.OfType<SistemaRefrigeracion>().FirstOrDefault(sr => sr.Nombre == nombre);
        if (sistemaRefrigeracion != null)
        {
            sistemaRefrigeracion.Estado = EstadoComponente.Apagado;
            Console.WriteLine($"El sistema de refrigeración {nombre} ha sido apagado.");
        }
        else
        {
            Console.WriteLine("No se encontró el sistema de refrigeración con el nombre proporcionado.");
        }
    }



    public void EscribirReactoresEnArchivo()
    {
        // Implementar la lógica para escribir los reactores en un archivo, ordenados por nombre.
        var reactoresOrdenados = componentes.OfType<Reactor>().OrderBy(r => r.Nombre).ToList();
        try
        {
            using (StreamWriter file = new StreamWriter("D:\\1_DAW\\Programación\\CentralNuclear\\CentralNuclear.txt"))
            {
                foreach (Reactor reactor in reactoresOrdenados)
                {
                    file.WriteLine(reactor.ToString());
                }
            }
        }
        catch (Exception ex)
        {
            // Manejar la excepción
            Console.WriteLine("Ocurrió un error al intentar escribir los reactores en el archivo: " + ex.Message);
        }
    }

    public void LeerReactoresDesdeArchivo()
    {
        // Implementar la lógica para leer los reactores desde un archivo.
        try
        {
            using (StreamReader file = new StreamReader("D:\\1_DAW\\Programación\\CentralNuclear\\CentralNuclear.txt"))
            {
                string line;
                while ((line = file.ReadLine()) != null)
                {
                    Console.WriteLine(line);
                }
            }
        }
        catch (Exception ex)
        {
            // Manejar la excepción
            Console.WriteLine("Ocurrió un error al intentar escribir los reactores en el archivo: " + ex.Message);
        }
    }

    public void GuardarDatosEnTexto()
    {
        // Implementar la lógica para guardar datos en texto.
        try
        {
            using (StreamWriter file = new StreamWriter("D:\\1_DAW\\Programación\\CentralNuclear\\DatosCentralNuclear.txt"))
            {
                foreach (ComponenteNuclear componente in componentes)
                {
                    file.WriteLine(componente.ToString());
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Ocurrió un error al intentar escribir los reactores en el archivo: " + ex.Message);
        }
    }

 
}

class Program
{
    static void Main(string[] args)
    {
        CentralNuclear central = new CentralNuclear();

        // Añadir componentes a la central
        central.AgregarComponente(new Reactor("R1", EstadoComponente.Encendido, 100));
        central.AgregarComponente(new SistemaRefrigeracion("SR1", EstadoComponente.Apagado, 35.5));
        central.AgregarComponente(new SistemaRefrigeracion("SR2", EstadoComponente.Encendido, 40.0));
        central.AgregarComponente(new Reactor("R2", EstadoComponente.Encendido, 120));
        central.AgregarComponente(new SistemaRefrigeracion("SR3", EstadoComponente.Apagado, 37.0));
        
        /*
        // Mostrar el estado inicial de la central
        Console.WriteLine("Estado inicial de la central nuclear:");
        central.MostrarEstadoCentral();

        // Realizar mantenimiento
        central.RealizarMantenimientoGeneral();

        // Mostrar el estado después del mantenimiento
        Console.WriteLine("\nEstado de la central después del mantenimiento:");
        central.MostrarEstadoCentral();
        */

        bool salir = false;
        while (!salir)
        {
            Console.WriteLine("***************************************************************************************");
            Console.WriteLine("            MENÚ");
            Console.WriteLine("***************************************************************************************");
            Console.WriteLine("1. Mostrar todos los componentes");
            Console.WriteLine("2. Encender Reactor");
            Console.WriteLine("3. Apagar Reactor");
            Console.WriteLine("4. Agregar Componente");
            Console.WriteLine("5. Realizar Mantenimiento");
            Console.WriteLine("6. Regular Temperatura del Reactor");
            Console.WriteLine("7. Escribir en un archivo (CentralNuclear.txt) Reactores ordenados por nombre");
            Console.WriteLine("8. Leer en un archivo (CentralNuclear.txt) Reactores ordenados por nombre");
            Console.WriteLine("9. Guardar Datos en Texto");
            Console.WriteLine("10. Salir");
            Console.WriteLine("11. Encender sistema de refrigeración");
            Console.WriteLine("12. Apagar sistema de refrigeración");

            Console.Write("Seleccione una opción: ");

            int opcion = Convert.ToInt32(Console.ReadLine());

            switch (opcion)
            {
                case 1:
                    central.MostrarEstadoCentral();
                    break;
                case 2:
                    // Pedir ID del reactor a encender y llamar a EncenderReactor
                    Console.Write("Ingrese el nombre del reactor a encender: ");
                    string nombreReactorEncender = Console.ReadLine();
                    central.EncenderReactor(nombreReactorEncender);
                    break;
                case 3:
                    // Pedir ID del reactor a apagar y llamar a ApagarReactor
                    Console.Write("Ingrese el nombre del reactor a apagar: ");
                    string nombreReactorApagar = Console.ReadLine();
                    central.ApagarReactor(nombreReactorApagar);
                    break;
                case 4:
                    // Pedir datos del componente a agregar y llamar a AgregarComponente
                    // Aquí deberías agregar el código para pedir los datos del componente
                    // y llamar al método AgregarComponente
                    Console.Write("Ingrese el nombre del componente: ");
                    string nombreComponente = Console.ReadLine();
                    Console.Write("Ingrese el estado del componente (Encendido, Apagado, Mantenimiento): ");
                    string estadoComponente = Console.ReadLine();
                    Console.Write("Ingrese el nivel de radiación (solo para reactores): ");
                    double nivelRadiacion = Convert.ToDouble(Console.ReadLine());
                    Console.Write("Ingrese la temperatura (solo para sistemas de refrigeración): ");
                    double temperatura = Convert.ToDouble(Console.ReadLine());
                    if (estadoComponente == "Encendido")
                    {
                        central.AgregarComponente(new Reactor(nombreComponente, EstadoComponente.Encendido, nivelRadiacion));
                    }
                    else if (estadoComponente == "Apagado")
                    {
                        central.AgregarComponente(new Reactor(nombreComponente, EstadoComponente.Apagado, nivelRadiacion));
                    }
                    else if (estadoComponente == "Mantenimiento")
                    {
                        central.AgregarComponente(new Reactor(nombreComponente, EstadoComponente.Mantenimiento, nivelRadiacion));
                    }
                    else
                    {
                        central.AgregarComponente(new SistemaRefrigeracion(nombreComponente, EstadoComponente.Encendido, temperatura));
                    }
                    break;
                case 5:
                    central.RealizarMantenimientoGeneral();
                    break;
                case 6:
                    // Pedir ID del reactor y temperatura para regular y llamar a RegularTemperaturaReactor
                    Console.Write("Ingrese el nombre del reactor a regular: ");
                    string nombreReactorRegular = Console.ReadLine();
                    Console.Write("Ingrese la nueva temperatura: ");
                    double nuevaTemperatura = Convert.ToDouble(Console.ReadLine());
                    central.RegularTemperaturaReactor(nombreReactorRegular, nuevaTemperatura);
                    break;
                case 7:
                    central.EscribirReactoresEnArchivo();
                    break;
                case 8:
                    central.LeerReactoresDesdeArchivo();
                    break;
                case 9:
                    central.GuardarDatosEnTexto();
                    break;
                case 10:
                    salir = true;
                    break;
                case 11:
                    Console.Write("Ingrese el nombre del sistema de refrigeración a encender: ");
                    string nombreSistemaRefrigeracionEncender = Console.ReadLine();
                    central.EncenderSistemaRefrigeracion(nombreSistemaRefrigeracionEncender);
                    break;
                case 12:
                    Console.Write("Ingrese el nombre del sistema de refrigeración a apagar: ");
                    string nombreSistemaRefrigeracionApagar = Console.ReadLine();
                    central.ApagarSistemaRefrigeracion(nombreSistemaRefrigeracionApagar);
                    break;

                default:
                    Console.WriteLine("Opción no válida. Por favor, intente de nuevo.");
                    break;
            }

        }
    }
}


