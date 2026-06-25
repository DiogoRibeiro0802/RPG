using System;
using System.Collections.Generic;
/* Realizado por: Diogo Ribeiro 
 *                Brian Silva
 */
class Personagem
{
    private int _vida;

    public string Nome { get; set; }
    public int VidaMaxima { get; set; }
    public int Vida
    {
        get { return _vida; }
        set { _vida = Math.Max(0, Math.Min(value, VidaMaxima)); }
    }
    public bool EstaVivo => Vida > 0;
    public Habilidade Habilidade1 { get; set; }
    public Habilidade Habilidade2 { get; set; }

    public Personagem(string nome, int vidaMaxima)
    {
        Nome = nome;
        VidaMaxima = vidaMaxima;
        Vida = vidaMaxima;
    }

    public virtual int Atacar()
    {
        Console.WriteLine($"  {Nome} ataca!");
        return 10;
    }

    public void ReceberDano(int dano)
    {
        Vida -= dano;
        Console.WriteLine($"  {Nome} recebeu {dano} de dano! (Vida: {Vida}/{VidaMaxima})");
    }

    public void Curar(int quantidade)
    {
        Vida += quantidade;
        Console.WriteLine($"  {Nome} recuperou {quantidade} de vida! (Vida: {Vida}/{VidaMaxima})");
    }

    public void Resetar() => Vida = VidaMaxima;

    public virtual string ObterTipo() => "Personagem";
}

class Guerreiro : Personagem
{
    public int Forca { get; set; }
    private Random _random = new Random();

    public Guerreiro(string nome) : base(nome, 150)
    {
        Forca = 25;
        Habilidade1 = new Habilidade("Golpe Brutal", dano: 45, cura: 0);
        Habilidade2 = new Habilidade("Grito de Guerra", dano: 20, cura: 10);
    }

    public override int Atacar()
    {
        int dano = Forca + _random.Next(0, 10);
        Console.WriteLine($"  {Nome} ataca com a espada! Dano: {dano}");
        return dano;
    }

    public override string ObterTipo() => "Guerreiro";
}

class Mago : Personagem
{
    public int PoderMagico { get; set; }
    private Random _random = new Random();

    public Mago(string nome) : base(nome, 90)
    {
        PoderMagico = 35;
        Habilidade1 = new Habilidade("Bola de Fogo", dano: 50, cura: 0);
        Habilidade2 = new Habilidade("Drenar Vida", dano: 20, cura: 20);
    }

    public override int Atacar()
    {
        int dano = PoderMagico + _random.Next(-5, 10);
        Console.WriteLine($"  {Nome} lanca um feitico! Dano: {dano}");
        return dano;
    }

    public override string ObterTipo() => "Mago";
}

class Arqueiro : Personagem
{
    public int Destreza { get; set; }
    private Random _random = new Random();

    public Arqueiro(string nome) : base(nome, 110)
    {
        Destreza = 22;
        Habilidade1 = new Habilidade("Flecha Certeira", dano: 35, cura: 0);
        Habilidade2 = new Habilidade("Tiro Duplo", dano: 40, cura: 0);
    }

    public override int Atacar()
    {
        int dano = Destreza + _random.Next(0, 8);

        if (_random.NextDouble() < 0.25)
        {
            Console.WriteLine($"  {Nome} dispara uma flecha! CRITICO! Dano: {dano * 2}");
            return dano * 2;
        }

        Console.WriteLine($"  {Nome} dispara uma flecha! Dano: {dano}");
        return dano;
    }

    public override string ObterTipo() => "Arqueiro";
}

class Habilidade
{
    public string Nome { get; set; }
    public int Dano { get; set; }
    public int Cura { get; set; }

    public Habilidade(string nome, int dano, int cura)
    {
        Nome = nome;
        Dano = dano;
        Cura = cura;
    }

    public void Usar(Personagem utilizador, Personagem alvo)
    {
        Console.WriteLine($"  {utilizador.Nome} usa [{Nome}]!");
        if (Dano > 0) alvo.ReceberDano(Dano);
        if (Cura > 0) utilizador.Curar(Cura);
    }
}

class Pocao
{
    public string Nome { get; set; }
    private int _cura;

    public Pocao(string nome, int cura)
    {
        Nome = nome;
        _cura = cura;
    }

    public void Usar(Personagem alvo)
    {
        Console.WriteLine($"  {alvo.Nome} usou {Nome}!");
        alvo.Curar(_cura);
    }
}

class Program
{
    static Personagem[] personagens = new Personagem[10];
    static int totalPersonagens = 0;

    static List<string> historicoCombates = new List<string>();

    static void Main()
    {
        bool sair = false;

        while (!sair)
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("=== MENU PRINCIPAL ===");
            Console.WriteLine($"Personagens criados: {totalPersonagens}");
            Console.WriteLine("[1] Criar Personagem");
            Console.WriteLine("[2] Ver Personagens");
            Console.WriteLine("[3] Iniciar Combate 1v1");
            Console.WriteLine("[4] Ver Resultados dos Combates (Novo!)");
            Console.WriteLine("[0] Sair");
            Console.Write("Opcao: ");
            string opcao = Console.ReadLine();

            if (opcao == "1") CriarPersonagem();
            else if (opcao == "2") VerPersonagensComPausa(); 
            else if (opcao == "3") IniciarCombate();
            else if (opcao == "4") VerHistorico();
            else if (opcao == "0") sair = true;
            else
            {
                Console.WriteLine("Opcao invalida! Pressione qualquer tecla...");
                Console.ReadKey();
            }
        }
    }

    static void CriarPersonagem()
    {
        Console.Clear();
        if (totalPersonagens >= 10)
        {
            Console.WriteLine("Limite de personagens atingido!");
            Console.ReadKey();
            return;
        }

        Console.WriteLine();
        Console.WriteLine("=== CRIAR PERSONAGEM ===");
        Console.Write("Nome: ");
        string nome = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(nome))
        {
            Console.WriteLine("Nome invalido!");
            Console.ReadKey();
            return;
        }

        Console.WriteLine("[1] Guerreiro  (150 HP | Ataque fisico forte)");
        Console.WriteLine("[2] Mago       ( 90 HP | Magia muito poderosa)");
        Console.WriteLine("[3] Arqueiro   (110 HP | Chance de critico x2)");
        Console.Write("Classe: ");
        string classe = Console.ReadLine();

        if (classe == "1") personagens[totalPersonagens++] = new Guerreiro(nome);
        else if (classe == "2") 
            personagens[totalPersonagens++] = new Mago(nome);
        else if (classe == "3") personagens[totalPersonagens++] = new Arqueiro(nome);
        else { Console.WriteLine("Classe invalida!"); Console.ReadKey(); return; }

        Console.WriteLine($"Personagem '{nome}' criado com sucesso!");
        Console.WriteLine("Pressione qualquer tecla para voltar...");
        Console.ReadKey();
    }

    static void VerPersonagensComPausa()
    {
        VerPersonagens();
        Console.WriteLine("\nPressione qualquer tecla para voltar...");
        Console.ReadKey();
    }

    static void VerPersonagens()
    {
        Console.Clear();
        Console.WriteLine();
        Console.WriteLine("=== PERSONAGENS ===");

        if (totalPersonagens == 0)
        {
            Console.WriteLine("Nenhum personagem criado ainda.");
            return;
        }

        for (int i = 0; i < totalPersonagens; i++)
        {
            Personagem p = personagens[i];
            Console.WriteLine($"[{i + 1}] {p.ObterTipo()} - {p.Nome} | HP: {p.VidaMaxima}");
            Console.WriteLine($"     Habilidade 1: {p.Habilidade1.Nome} (Dano:{p.Habilidade1.Dano} Cura:{p.Habilidade1.Cura})");
            Console.WriteLine($"     Habilidade 2: {p.Habilidade2.Nome} (Dano:{p.Habilidade2.Dano} Cura:{p.Habilidade2.Cura})");
            Console.WriteLine(new string('-', 40));
        }
    }

    static void IniciarCombate()
    {
        Console.Clear();
        Console.WriteLine();

        if (totalPersonagens < 2)
        {
            Console.WriteLine("Precisas de pelo menos 2 personagens!");
            Console.ReadKey();
            return;
        }

        VerPersonagens();
        Console.WriteLine();
        Console.Write("Escolhe o Jogador 1 (numero): ");

        int idx1;
        if (!int.TryParse(Console.ReadLine(), out idx1) || idx1 <= 0 || idx1 > totalPersonagens)
        {
            Console.WriteLine("Selecao invalida!");
            Console.ReadKey();
            return;
        }
        idx1--;

        Console.Write("Escolhe o Jogador 2 (numero): ");
        int idx2;
        if (!int.TryParse(Console.ReadLine(), out idx2) || idx2 <= 0 || idx2 > totalPersonagens || (idx2 - 1) == idx1)
        {
            Console.WriteLine("Selecao invalida ou repetida!");
            Console.ReadKey();
            return;
        }
        idx2--;

        Personagem p1 = personagens[idx1];
        Personagem p2 = personagens[idx2];

        p1.Resetar();
        p2.Resetar();

        p1.Resetar();
        p2.Resetar();

        Pocao[] inv1 = new Pocao[] { new Pocao("Pocao Pequena", 30), new Pocao("Pocao Grande", 60) };
        Pocao[] inv2 = new Pocao[] { new Pocao("Pocao Pequena", 30), new Pocao("Pocao Grande", 60) };
        int inv1Usadas = 0;
        int inv2Usadas = 0;

        Console.WriteLine();
        Console.WriteLine("=== COMBATE INICIADO ===");
        Console.WriteLine($"{p1.ObterTipo()} {p1.Nome} VS {p2.ObterTipo()} {p2.Nome}");

        int turno = 1;

        while (p1.EstaVivo && p2.EstaVivo)
        {
            Console.WriteLine();
            Console.WriteLine($"--- TURNO {turno} ---");
            Console.WriteLine($"{p1.Nome}: {p1.Vida}/{p1.VidaMaxima} HP  |  {p2.Nome}: {p2.Vida}/{p2.VidaMaxima} HP");

            if (p1.EstaVivo)
            {
                Console.WriteLine();
                Console.WriteLine($"[Turno de {p1.Nome}]");
                FazerAcao(p1, p2, inv1, ref inv1Usadas);
            }

            if (p2.EstaVivo && p1.EstaVivo) 
            {
                Console.WriteLine();
                Console.WriteLine($"[Turno de {p2.Nome}]");
                FazerAcao(p2, p1, inv2, ref inv2Usadas);
            }

            turno++;
        }

        Console.WriteLine();
        Console.WriteLine("=== FIM DO COMBATE ===");

        string resultadoLog = "";
        if (p1.EstaVivo)
        {
            Console.WriteLine($"{p1.Nome} venceu o combate!");
            resultadoLog = $"[VITORIA] {p1.Nome} ({p1.ObterTipo()}) derrotou {p2.Nome} ({p2.ObterTipo()}) em {turno - 1} turnos.";
        }
        else
        {
            Console.WriteLine($"{p2.Nome} venceu o combate!");
            resultadoLog = $"[VITORIA] {p2.Nome} ({p2.ObterTipo()}) derrotou {p1.Nome} ({p1.ObterTipo()}) em {turno - 1} turnos.";
        }

        historicoCombates.Add(resultadoLog);

        Console.WriteLine("\nPressione qualquer tecla para voltar ao menu...");
        Console.ReadKey();
    }

    static void VerHistorico()
    {
        Console.Clear();
        Console.WriteLine();
        Console.WriteLine("=== HISTÓRICO DE COMBATES ===");

        if (historicoCombates.Count == 0)
        {
            Console.WriteLine("Nenhum combate foi realizado ainda nesta sessão.");
        }
        else
        {
            for (int i = 0; i < historicoCombates.Count; i++)
            {
                Console.WriteLine($"Combate #{i + 1}: {historicoCombates[i]}");
            }
        }

        Console.WriteLine("\nPressione qualquer tecla para voltar ao menu...");
        Console.ReadKey();
    }

    static void FazerAcao(Personagem atacante, Personagem alvo, Pocao[] inventario, ref int usadas)
    {
        bool acaoFeita = false;

        while (!acaoFeita)
        {
            Console.WriteLine("  [1] Atacar");
            Console.WriteLine("  [2] Usar Habilidade");
            Console.WriteLine("  [3] Usar Item");
            Console.Write("  Escolha: ");
            string opcao = Console.ReadLine();

            if (opcao == "1")
            {
                int dano = atacante.Atacar();
                alvo.ReceberDano(dano);
                acaoFeita = true;
            }
            else if (opcao == "2")
            {
                Console.WriteLine($"  [1] {atacante.Habilidade1.Nome} (Dano:{atacante.Habilidade1.Dano} Cura:{atacante.Habilidade1.Cura})");
                Console.WriteLine($"  [2] {atacante.Habilidade2.Nome} (Dano:{atacante.Habilidade2.Dano} Cura:{atacante.Habilidade2.Cura})");
                Console.Write("  Escolha: ");
                string hi = Console.ReadLine();

                if (hi == "1") { atacante.Habilidade1.Usar(atacante, alvo); acaoFeita = true; }
                else if (hi == "2") { atacante.Habilidade2.Usar(atacante, alvo); acaoFeita = true; }
                else Console.WriteLine("  Opcao invalida!");
            }
            else if (opcao == "3")
            {
                if (usadas >= inventario.Length)
                {
                    Console.WriteLine("  Inventario vazio!");
                }
                else
                {
                    for (int i = usadas; i < inventario.Length; i++)
                        Console.WriteLine($"  [{i - usadas + 1}] {inventario[i].Nome}");

                    Console.Write("  Escolha: ");
                    int entrada;
                    if (int.TryParse(Console.ReadLine(), out entrada))
                    {
                        int ii = entrada - 1 + usadas;
                        if (ii >= usadas && ii < inventario.Length)
                        {
                            inventario[ii].Usar(atacante);
                            Pocao temp = inventario[usadas];
                            inventario[usadas] = inventario[ii];
                            inventario[ii] = temp;
                            usadas++;
                            acaoFeita = true;
                        }
                        else Console.WriteLine("  Opcao invalida!");
                    }
                    else Console.WriteLine("  Opcao invalida!");
                }
            }
            else Console.WriteLine("  Opcao invalida!");
        }
    }
}