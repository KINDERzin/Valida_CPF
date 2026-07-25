using System.Collections;

namespace ValidaCPF;

public partial class MainPage : ContentPage
{
	bool isValid;
	//Guarda o valor das multiplicações do CPF
	int resultadoMultiplicacao;
	//Número que efetuará a multiplicação
	int multiplicador;
	//Passa como parametro o local do algarismo no array
	int lugarArray;
	int[] cpfEntrada;

	public MainPage()
	{
		InitializeComponent();
	}

	private void initializeVariables()
	{
		isValid = false;
		resultadoMultiplicacao = 0;
		multiplicador = 2;
		lugarArray = 8;
	}

	private void validarCpf(string cpf)
	{
		initializeVariables();

		string cpfLimpo = new string(cpf.Where(char.IsDigit).ToArray());
		if (cpfLimpo.Length != 11) {
			labelCpf.Text = "Você não sabe digitar um CPF????";
			return;
		}

		//Array - Tipo_Do_Array[] Nome = new Tipo_Do_Array[Tamanho_Do_Array];
		//Pega cada algarismo e transforma em um valor de array 
		cpfEntrada   = cpfLimpo.ToString().Select(c => int.Parse(c.ToString())).ToArray();
		int[] CpfCalculado = cpfLimpo.ToString().Select(a => int.Parse(a.ToString())).ToArray();
	
		labelCpf.Text = $"Seu CPF é: {entryCpf.Text}"; 

		//Realiza o calculo do 10º dígito do CPF
		int multiplicacao;
		for(int i = 0; i < 8; i++) {
			multiplicacao = CpfCalculado[lugarArray] * multiplicador;
			resultadoMultiplicacao += multiplicacao;
			lugarArray--;
			multiplicador++;
		}

		labelMult.Text = $"Resultado da multiplicação: {resultadoMultiplicacao.ToString()}";

		var restoDivisao = resultadoMultiplicacao % 11;
		int penultimoDigito = 0;

		if(restoDivisao < 2) {
			penultimoDigito = 0;
			CpfCalculado[9] = penultimoDigito;
		}
		else {
			penultimoDigito = 11 - restoDivisao;
			CpfCalculado[9] = penultimoDigito;
		}

		multiplicador = 2;
		lugarArray = 9;
		resultadoMultiplicacao = 0;

		multiplicacao = 0;
		for(int i = 0; i < 9; i++) {
			multiplicacao = CpfCalculado[lugarArray] * multiplicador;
			resultadoMultiplicacao += multiplicacao;
			lugarArray--;
			multiplicador++;
		}
		restoDivisao = resultadoMultiplicacao % 11;
		
		int ultimoDigito = 0;

		if(restoDivisao < 2) {
			ultimoDigito = 0;
			CpfCalculado[10] = ultimoDigito;
		}
		else {
			ultimoDigito = 11 - restoDivisao;
			CpfCalculado[10] = ultimoDigito;
		}

		isValid = CpfCalculado[9] == cpfEntrada[9] && CpfCalculado[10] == cpfEntrada[10];

		iconResultado.IsVisible = true;
		iconResultado.Text = isValid ? "✅" : "❌";

		labelResultado.Text = isValid ? "VÁLIDO" : "INVÁLIDO";
		labelResultado.TextColor = isValid
			? Color.FromArgb("#059669")
			: Color.FromArgb("#DC2626");

		frameResultado.BackgroundColor = isValid
			? Color.FromArgb("#ECFDF5")
			: Color.FromArgb("#FEF2F2");
			

		labelPenultimo.Text = $"O penultimo dígito é: {penultimoDigito.ToString()}";
		labelUltimo.Text = $"O último dígito é: {ultimoDigito.ToString()}";
	}

	private bool percorreCpf(string cpf)
	{
		for(int i = 1; i < cpf.Length; i++) {
			if(cpf[i] != cpf[0]) {
				return false;
			}
		}
		return true;
	}

	private void clearButton(object sender, EventArgs e)
	{
		entryCpf.Text = string.Empty;
		labelCpf.Text = string.Empty;
		labelMult.Text = string.Empty;
		labelPenultimo.Text = string.Empty;
		labelUltimo.Text = string.Empty;
		labelResultado.Text = string.Empty;
	}

	private void ConfirmButton(object sender, EventArgs e)
	{
		frameResultado.IsVisible = true;

		if(string.IsNullOrEmpty(entryCpf.Text)) {
			labelCpf.Text = "É PARA DIGITAR UM CPF!!!!";
			labelResultado.Text = "Are you dumb man???"; 
			labelResultado.TextColor = Color.FromArgb("#DC2626");
			return;
		}

		if(percorreCpf(new string(entryCpf.Text.Where(char.IsDigit).ToArray()))) {
			labelResultado.Text = "BRUTAL...., get mogged";
			labelResultado.TextColor = Color.FromArgb("#DC2626");
			iconResultado.IsVisible = true;
			iconResultado.Text = "❌";
			labelCpf.Text = "Pode não paizão, é inválido!";
			frameResultado.BackgroundColor = Color.FromArgb("#FEF2F2");
			return;
		}
		validarCpf(entryCpf.Text);
		if(isValid) {
			labelRegiao.IsVisible = true;
			labelRegiao.Text = $"Região fiscal: {getRegion(cpfEntrada[8].ToString()[0])}";
		}
		else
			labelRegiao.IsVisible = false;
	}

	private string getRegion(char ninthDigit)
	{
		return ninthDigit switch
		{
			'0' => "RS",
        	'1' => "DF, GO, MS, MT, TO",
        	'2' => "AC, AM, AP, PA, RO, RR",
        	'3' => "CE, MA, PI",
        	'4' => "AL, PB, PE, RN",
        	'5' => "BA, SE",
        	'6' => "MG",
       	'7' => "ES, RJ",
      	'8' => "SP",
      	'9' => "PR, SC",
       	_   => "Desconhecida"
		};
	}
}