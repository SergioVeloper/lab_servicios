<?php

namespace App\Http\Controllers;

use Illuminate\Http\Request;
use Illuminate\Support\Facades\Log; // Importa la clase Log
use App\Models\Cotizacion;

class CotizacionController extends Controller
{
    public function index()
    {
        $cotizaciones = Cotizacion::all();
        return response()->json($cotizaciones, 200);
    }

    public function store(Request $request)
    {
        $cotizacion = Cotizacion::create($request->all());
        return response()->json($cotizacion, 201);
    }

    public function show($id)
    {
        $cotizacion = Cotizacion::find($id);
        if($cotizacion){
            return response()->json($cotizacion, 200);
        } else {
            return response()->json(null, 404);
        }
    }

    public function update(Request $request, $id)
    {
        $cotizacion = Cotizacion::find($id);
        if($cotizacion){
            $cotizacion->update($request->all());
            return response()->json($cotizacion, 200);
        } else {
            return response()->json(null, 404);
        }
    }

    public function destroy($id)
    {
        $cotizacion = Cotizacion::find($id);
        if($cotizacion){
            $cotizacion->delete();
            return response()->json($cotizacion, 200);
        } else {
            return response()->json(null, 404);
        }
    }

    public function getCotizacionPorFecha(Request $request)
    {
        Log::info('Esto es un mensaje de prueba para verificar el logging.');
        $fecha = $request->query('fecha'); // Usar 'query' para obtener el parámetro de la URL

        // Registrar para confirmar que recibimos la fecha
        Log::info("Fecha recibida en el controlador: " . $fecha);

        // Filtrar la cotización que coincide con la fecha proporcionada
        $cotizacion = Cotizacion::whereDate('fecha', $fecha)->get();

        // Registrar el resultado de la consulta
        Log::info("Resultado de la consulta: " . $cotizacion);

        return response()->json($cotizacion);
    }
}
