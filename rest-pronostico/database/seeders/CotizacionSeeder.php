<?php

namespace Database\Seeders;

use Illuminate\Database\Console\Seeds\WithoutModelEvents;
use Illuminate\Database\Seeder;
use App\Models\Cotizacion;

class CotizacionSeeder extends Seeder
{
    /**
     * Run the database seeds.
     */
    public function run(): void
    {
        $datos = [
            ['fecha' => '2024-06-04', 'cotizacion' => 7.02, 'cotizacion_oficial' => 6.97],
            ['fecha' => '2024-06-05', 'cotizacion' => 7.05, 'cotizacion_oficial' => 6.97],
            ['fecha' => '2024-06-06', 'cotizacion' => 7.10, 'cotizacion_oficial' => 6.97],
            ['fecha' => '2024-06-07', 'cotizacion' => 8.20, 'cotizacion_oficial' => 6.97],
            ['fecha' => '2024-06-08', 'cotizacion' => 8.30, 'cotizacion_oficial' => 6.97],
            ['fecha' => '2024-06-09', 'cotizacion' => 8.40, 'cotizacion_oficial' => 6.97],
            ['fecha' => '2024-06-10', 'cotizacion' => 8.70, 'cotizacion_oficial' => 6.97],
            ['fecha' => '2024-06-11', 'cotizacion' => 9.00, 'cotizacion_oficial' => 6.97],
            ['fecha' => '2024-06-12', 'cotizacion' => 9.20, 'cotizacion_oficial' => 6.97],
        ];

        foreach ($datos as $dato) {
            Cotizacion::create($dato);
        }
    }
}
