<?php

namespace App\Models;

use Illuminate\Database\Eloquent\Factories\HasFactory;
use Illuminate\Database\Eloquent\Model;

class cotizacion extends Model
{
    /** @use HasFactory<\Database\Factories\CotizacionFactory> */
    use HasFactory;
    protected $table = 'cotizaciones';
    protected $fillable = ['fecha', 'cotizacion', 'cotizacion_oficial'];
}
