<?php

namespace App\Http\Controllers;

use Illuminate\Http\Request;
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
        }else{
            return response()->json(null, 404);
        }
    }

    public function update(Request $request, $id)
    {
        $cotizacion = Cotizacion::find($id);
        if($cotizacion){
            $cotizacion->update($request->all());
            return response()->json($cotizacion, 200);
        }else{
            return response()->json(null, 404);
        }
    }

    public function destroy($id)
    {
        $cotizacion = Cotizacion::find($id);
        if($cotizacion){
            $cotizacion->delete();
            return response()->json($cotizacion, 200);
        }else{
            return response()->json(null, 404);
        }
    }
}
