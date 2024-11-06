namespace ValidaCPF;

public partial class MainPage : ContentPage
{
	//Guarda o valor das multiplicações do CPF
	int resultadoMultiplicacao = 0;
	//Número que efetuará a multiplicação
	int multiplicador = 2;
	//Passa como parametro o local do algarismo no array
	int lugarArray = 8;

	public MainPage()
	{
		InitializeComponent();
	}

	private void ConfirmButton(object sender, EventArgs e)
	{
		//Array - Tipo_Do_Array[] Nome = new Tipo_Do_Array[Tamanho_Do_Array];
		string entrada = entryCpf.Text;
		//Pega cada algarismo e transforma em um valor de array 
		int[] Cpf = entrada.ToString().Select(a => int.Parse(a.ToString())).ToArray();
		
		// .Substring(Posição do caractere, quantidade de caracteres);
		var cpfFormatado = entrada.Substring(0, 3) + "." + entrada.Substring(3, 3) + "." + entrada.Substring(3, 3) + "-" + entrada.Substring(9, 2);
		labelCpf.Text = $"Seu CPF é: {cpfFormatado}";

		//Realiza o calculo do 10º dígito do CPF
		for(int i = 0; i < 8; i++)
		{
			int multiplicacao = Cpf[lugarArray] * multiplicador;
			resultadoMultiplicacao = multiplicacao + resultadoMultiplicacao;
			lugarArray--;
			multiplicador++;
		}

		labelMult.Text = $"Resultado da multiplicação: {resultadoMultiplicacao.ToString()}";

		var restoDivisao = resultadoMultiplicacao % 11;
		int n;
		if(restoDivisao < 2)
		{
			n = 0;
			Cpf[9] = n;
		}
		else{
			n = 11 - restoDivisao;
			Cpf[9] = n;
		}
		labelPenultimo.Text = $"O penultimo dígito é: {n.ToString()}";

		multiplicador = 2;
		lugarArray = 9;
		resultadoMultiplicacao = 0;
		for(int i = 0; i < 9; i++)
		{
			int multiplicacao = Cpf[lugarArray] * multiplicador;
			resultadoMultiplicacao = multiplicacao + resultadoMultiplicacao;
			lugarArray--;
			multiplicador++;
		}
		restoDivisao = resultadoMultiplicacao % 11;

		if(restoDivisao < 2)
		{
			n = 0;
			Cpf[10] = n;
		}
		else{
			n = 11 - restoDivisao;
			Cpf[10] = n;
		}
		labelUltimo.Text = $"O último dígito é: {n.ToString()}";
	}
}