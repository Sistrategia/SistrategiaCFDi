using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistrategia.SAT.CFDiWebSite.CFDI
{
    public class CantidadEnLetraConverter
    {
        /**
                 * Propiedades:
                 * $numero: Es la cantidad a ser convertida a letras maximo 999,999,999,999.99
                 * $genero: 0 para femenino y 1 para masculino, es util dependiendo de la
                 * moneda ej: cuatrocientos pesos / cuatrocientas pesetas
                 * $moneda: nombre de la moneda
                 * $prefijo: texto a imprimir antes de la cantidad
                 * $sufijo: texto a imprimir despues de la cantidad
                 * tanto el $sufijo como el $prefijo en la impresion de cheques o
                 * facturas, para impedir que se altere la cantidad
                 * $mayusculas: 0 para minusculas, 1 para mayusculas indica como debe
                 * mostrarse el texto
                 * $textos_posibles: contiene todas las posibles palabras a usar
                 * $aTexto: es el arreglo de los textos que se usan de acuerdo al genero
                 * seleccionado
                 */

        private decimal numero = 0;
        private int genero = 1;
        private string moneda = "PESOS";
        private string prefijo = ""; //"(***";
        private string sufijo = ""; //"***)";
        private bool mayusculas = true;

        // textos
        private Dictionary<int, string[]> textosPosibles = new Dictionary<int, string[]>();

        private string[][] aTexto = new string[7][];

        /**
         * Metodos:
         * _construct: Inicializa textos
         * setNumero: Asigna el numero a convertir a letra
         * setPrefijo: Asigna el prefijo
         * setSufijo: Asiga el sufijo
         * setMoneda: Asigna la moneda
         * setGenero: Asigan genero
         * setMayusculas: Asigna uso de mayusculas o minusculas
         * letra: Convierte numero en letra
         * letraUnidad: Convierte unidad en letra, asigna miles y millones
         * letraDecena: Contiene decena en letra
         * letraCentena: Convierte centena en letra
         */
        public CantidadEnLetraConverter() {
            textosPosibles.Add(0, new string[] { "UNA ", "DOS ", "TRES ", "CUATRO ", "CINCO ", "SEIS ", "SIETE ", "OCHO ", "NUEVE ", "UN " });

            textosPosibles.Add(1, new string[] { "ONCE ", "DOCE ", "TRECE ", "CATORCE ", "QUINCE ", "DIECISEIS ", "DIECISIETE ", "DIECIOCHO ", "DIECINUEVE ", "" });
            textosPosibles.Add(2, new string[] { "DIEZ ", "VEINTE ", "TREINTA ", "CUARENTA ", "CINCUENTA ", "SESENTA ", "SETENTA ", "OCHENTA ", "NOVENTA ", "VEINTI" });
            textosPosibles.Add(3, new string[] { "CIEN ", "DOSCIENTAS ", "TRESCIENTAS ", "CUATROCIENTAS ", "QUINIENTAS ", "SEISCIENTAS ", "SETECIENTAS ", "OCHOCIENTAS ", "NOVECIENTAS ", "CIENTO " });
            textosPosibles.Add(4, new string[] { "CIEN ", "DOSCIENTOS ", "TRESCIENTOS ", "CUATROCIENTOS ", "QUINIENTOS ", "SEISCIENTOS ", "SETECIENTOS ", "OCHOCIENTOS ", "NOVECIENTOS ", "CIENTO " });
            textosPosibles.Add(5, new string[] { "MIL ", "MILLON ", "MILLONES ", "CERO ", "Y ", "UN ", "DOS ", "CON ", "", "" });

            for (int i = 0; i < 6; i++) {
                this.aTexto[i] = new string[10];

                //for( int j = 0; j < 10; j++)
                //{
                //    this.aTexto[i][j] = this.textosPosibles[$i][$j];
                //}
            }

            this.aTexto[0] = new string[] { "UNA ", "DOS ", "TRES ", "CUATRO ", "CINCO ", "SEIS ", "SIETE ", "OCHO ", "NUEVE ", "UN " };

            this.aTexto[1] = new string[] { "ONCE ", "DOCE ", "TRECE ", "CATORCE ", "QUINCE ", "DIECISEIS ", "DIECISIETE ", "DIECIOCHO ", "DIECINUEVE ", "" };
            this.aTexto[2] = new string[] { "DIEZ ", "VEINTE ", "TREINTA ", "CUARENTA ", "CINCUENTA ", "SESENTA ", "SETENTA ", "OCHENTA ", "NOVENTA ", "VEINTI" };
            this.aTexto[3] = new string[] { "CIEN ", "DOSCIENTAS ", "TRESCIENTAS ", "CUATROCIENTAS ", "QUINIENTAS ", "SEISCIENTAS ", "SETECIENTAS ", "OCHOCIENTAS ", "NOVECIENTAS ", "CIENTO " };
            this.aTexto[4] = new string[] { "CIEN ", "DOSCIENTOS ", "TRESCIENTOS ", "CUATROCIENTOS ", "QUINIENTOS ", "SEISCIENTOS ", "SETECIENTOS ", "OCHOCIENTOS ", "NOVECIENTOS ", "CIENTO " };
            this.aTexto[5] = new string[] { "MIL ", "MILLON ", "MILLONES ", "CERO ", "Y ", "UN ", "DOS ", "CON ", "", "" };


        }

        public decimal Numero {
            get { return this.numero; }
            set { this.numero = value; }
        }

        public string Prefijo {
            get { return this.prefijo; }
            set { this.prefijo = value; }
        }

        public string Sufijo {
            get { return this.sufijo; }
            set { this.sufijo = value; }
        }

        public string Moneda {
            get { return this.moneda; }
            set { this.moneda = value; }
        }

        public int Genero {
            get { return this.genero; }
            set { this.genero = value; }
        }

        public bool Mayusculas {
            get { return this.mayusculas; }
            set { this.mayusculas = value; }
        }

        public string letra() {
            if (this.genero == 1) { // masculino
                this.aTexto[0][0] = this.textosPosibles[5][5];
                for (int j = 0; j < 9; j++) {
                    this.aTexto[3][j] = this.aTexto[4][j];
                }
            }
            else { // femenino
                this.aTexto[0][0] = this.textosPosibles[0][0];
                for (int j = 0; j < 9; j++) {
                    this.aTexto[3][j] = this.aTexto[3][j];
                }
            }

            //string cnumero = string.Format("000000000000.00", this.numero); // "000000006235.50";
            string cnumero = this.numero.ToString("000000000000.00"); // "000000006235.50";
            string texto = "";
            if (cnumero.Length > 15) {
                texto = "Excede tamaño permitido";
            }
            else {
                bool hay_significativo = false;
                for (int pos = 0; pos < 12; pos++) {
                    // Control existencia Dígito significativo
                    if (!(hay_significativo) && (cnumero.Substring(pos, 1) == "0")) {
                    }
                    else {
                        hay_significativo = true;
                    }
                    // Detectar Tipo de Dígito
                    switch (pos % 3) {
                        case 0: texto += this.letraCentena(pos, cnumero);
                            break;
                        case 1: texto += this.letraDecena(pos, cnumero);
                            break;
                        case 2: texto += this.letraUnidad(pos, cnumero);
                            break;
                    }
                }

                // Detectar caso 0
                if (texto == "") { texto = this.aTexto[5][3]; }

                if (this.mayusculas) { // mayusculas
                    texto = (this.prefijo + texto + " " + this.moneda + " " + cnumero.Substring(cnumero.Length - 2) + "/100 " + this.sufijo).ToUpper();
                }
                else { // minusculas
                    texto = (this.prefijo + texto + " " + this.moneda + " " + cnumero.Substring(cnumero.Length - 2) + "/100 " + this.sufijo).ToLower();
                }
            }
            return texto;
        }

        public override string ToString() {
            return this.letra();
        }

        // traducir letra a unidad
        private string letraUnidad(int pos, string cnumero) {
            string unidad_texto = "";
            if (!((cnumero.Substring(pos, 1) == "0") ||
                                    (cnumero.Substring(pos - 1, 1) == "1") ||
                                    ((cnumero.Substring(pos - 2, 3) == "001") && ((pos == 2) || (pos == 8)))
                                    )
                            ) {
                if ((cnumero.Substring(pos, 1) == "1") && (pos <= 6)) {
                    unidad_texto += this.aTexto[0][9];
                }
                else {
                    unidad_texto += this.aTexto[0][int.Parse(cnumero.Substring(pos, 1)) - 1];
                }
            }
            //if ((($pos == 2) || ($pos == 8)) &&
            //    (substr($cnumero, $pos - 2, 3) != '000')) { // miles
            //if (substr($cnumero, $pos, 1) == '1') {
            //    $unidad_texto = substr($unidad_texto, 0, -2) . " ";
            //    $unidad_texto .= $this->aTexto[5][0];
            //} else {
            //    $unidad_texto .= $this->aTexto[5][0];
            //} 
            if (((pos == 2) || (pos == 8)) &&
                            (cnumero.Substring(pos - 2, 3) != "000")) { // miles
                if (cnumero.Substring(pos, 1) == "1") {
                    if (unidad_texto.Length >= 2)
                        unidad_texto = unidad_texto.Substring(0, unidad_texto.Length - 2) + " ";  // REVISAR BIEN ESTE CASO
                    unidad_texto += this.aTexto[5][0];
                }
                else {
                    unidad_texto += this.aTexto[5][0];
                }
            }
            if (pos == 5 && cnumero.Substring(pos - 2, 3) != "000") {
                if (cnumero.Substring(0, 6) == "000001") { // millones
                    unidad_texto += this.aTexto[5][1];
                }
                else {
                    unidad_texto += this.aTexto[5][2];
                }
            }
            return unidad_texto;
        }
        // traducir digito a decena
        private string letraDecena(int pos, string cnumero) {
            string decena_texto = "";
            if (cnumero.Length <= 0) {
                return "";
            }
            if (cnumero.Substring(pos, 1) == "0") {
                return "";
            }
            else if (cnumero.Substring(pos + 1, 1) == "0") {
                decena_texto += this.aTexto[2][int.Parse(cnumero.Substring(pos, 1)) - 1];
            }
            else if (cnumero.Substring(pos, 1) == "1") {
                decena_texto += this.aTexto[1][int.Parse(cnumero.Substring(pos + 1, 1)) - 1];
            }
            else if (cnumero.Substring(pos, 1) == "2") {
                decena_texto += this.aTexto[2][9];
            }
            else {
                decena_texto += this.aTexto[2][int.Parse(cnumero.Substring(pos, 1)) - 1] + this.aTexto[5][4];
            }
            return decena_texto;
        }


        // traducir digito centena
        private string letraCentena(int pos, string cnumero) {
            string centena_texto = "";

            if (cnumero.Substring(pos, 1) == "0")
                return "";

            int pos2 = 3;
            if ((cnumero.Substring(pos, 1) == "1") && (cnumero.Substring(pos + 1, 2) != "00")) {
                centena_texto += this.aTexto[pos2][9];
            }
            else {
                centena_texto += this.aTexto[pos2][int.Parse(cnumero.Substring(pos, 1)) - 1];
            }

            return centena_texto;
        }

    }
}
