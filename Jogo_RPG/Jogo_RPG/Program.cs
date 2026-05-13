
interface IUsavel
{
    string Nome { get; }
    void Usar(Personagem alvo);
}

abstract class Personagem
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
    public List<Habilidade> Habilidades { get; set; } = new List<Habilidade>();

    public Personagem(string nome, int vidaMaxima)
    {
        Nome = nome;
        VidaMaxima = vidaMaxima;
        Vida = vidaMaxima;
    }

    public abstract int Atacar();

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

    public void Resetar()
    {
        Vida = VidaMaxima;
    }

    public abstract string ObterTipo();
}

class Guerreiro : Personagem
{
    public int Forca { get; set; }
    private Random _random = new Random();

    public Guerreiro(string nome) : base(nome, 150)
    {
        Forca = 25;
        Habilidades.Add(new Habilidade("Golpe Brutal", dano: 45, cura: 0));
        Habilidades.Add(new Habilidade("Grito de Guerra", dano: 20, cura: 10));
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
        Habilidades.Add(new Habilidade("Bola de Fogo", dano: 50, cura: 0));
        Habilidades.Add(new Habilidade("Drenar Vida", dano: 20, cura: 20));
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
        Habilidades.Add(new Habilidade("Flecha Certeira", dano: 35, cura: 0));
        Habilidades.Add(new Habilidade("Tiro Duplo", dano: 40, cura: 0));
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


class Pocao : IUsavel
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
    static List<Personagem> personagens = new List<Personagem>();

    static void Main()
    {
        bool sair = false;

        while (!sair)
        {
            Console.WriteLine();
            Console.WriteLine("=== MENU PRINCIPAL ===");
            Console.WriteLine($"Personagens criados: {personagens.Count}");
            Console.WriteLine("[1] Criar Personagem");
            Console.WriteLine("[2] Ver Personagens");
            Console.WriteLine("[3] Iniciar Combate 1v1");
            Console.WriteLine("[0] Sair");
            Console.Write("Opcao: ");
            string opcao = Console.ReadLine();

            if (opcao == "1") CriarPersonagem();
            else if (opcao == "2") VerPersonagens();
            else if (opcao == "3") IniciarCombate();
            else if (opcao == "0") sair = true;
            else Console.WriteLine("Opcao invalida!");
        }
    }

    static void CriarPersonagem()
    {
        Console.WriteLine();
        Console.WriteLine("=== CRIAR PERSONAGEM ===");
        Console.Write("Nome: ");
        string nome = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(nome))
        {
            Console.WriteLine("Nome invalido!");
            return;
        }

        Console.WriteLine("[1] Guerreiro  (150 HP | Ataque fisico forte)");
        Console.WriteLine("[2] Mago       ( 90 HP | Magia muito poderosa)");
        Console.WriteLine("[3] Arqueiro   (110 HP | Chance de critico x2)");
        Console.Write("Classe: ");
        string classe = Console.ReadLine();

        Personagem novo = null;

        if (classe == "1") novo = new Guerreiro(nome);
        else if (classe == "2") novo = new Mago(nome);
        else if (classe == "3") novo = new Arqueiro(nome);
        else
        {
            Console.WriteLine("Classe invalida!");
            return;
        }

        personagens.Add(novo);
        Console.WriteLine($"Personagem {novo.ObterTipo()} '{novo.Nome}' criado!");
    }

 
    static void VerPersonagens()
    {
        Console.WriteLine();
        Console.WriteLine("=== PERSONAGENS ===");

        if (personagens.Count == 0)
        {
            Console.WriteLine("Nenhum personagem criado ainda.");
            return;
        }

        for (int i = 0; i < personagens.Count; i++)
        {
            Personagem p = personagens[i];
            Console.WriteLine($"[{i + 1}] {p.ObterTipo()} - {p.Nome} | HP: {p.VidaMaxima}");

            for (int j = 0; j < p.Habilidades.Count; j++)
            {
                Habilidade h = p.Habilidades[j];
                Console.WriteLine($"     Habilidade {j + 1}: {h.Nome} (Dano:{h.Dano} Cura:{h.Cura})");
            }
        }
    }
    static void IniciarCombate()
    {
        Console.WriteLine();

        if (personagens.Count < 2)
        {
            Console.WriteLine("Precisas de pelo menos 2 personagens!");
            return;
        }


        VerPersonagens();
        Console.WriteLine();
        Console.Write("Escolhe o Jogador 1 (numero): ");
        int idx1 = int.Parse(Console.ReadLine()) - 1;

        if (idx1 < 0 || idx1 >= personagens.Count)
        {
            Console.WriteLine("Selecao invalida!");
            return;
        }

        Console.Write("Escolhe o Jogador 2 (numero): ");
        int idx2 = int.Parse(Console.ReadLine()) - 1;

        if (idx2 < 0 || idx2 >= personagens.Count || idx2 == idx1)
        {
            Console.WriteLine("Selecao invalida!");
            return;
        }

        Personagem p1 = personagens[idx1];
        Personagem p2 = personagens[idx2];

        p1.Resetar();
        p2.Resetar();

        List<IUsavel> inventario1 = new List<IUsavel>();
        inventario1.Add(new Pocao("Pocao Pequena", 30));
        inventario1.Add(new Pocao("Pocao Grande", 60));

        List<IUsavel> inventario2 = new List<IUsavel>();
        inventario2.Add(new Pocao("Pocao Pequena", 30));
        inventario2.Add(new Pocao("Pocao Grande", 60));

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
                FazerAcao(p1, p2, inventario1);
            }


            if (p2.EstaVivo)
            {
                Console.WriteLine();
                Console.WriteLine($"[Turno de {p2.Nome}]");
                FazerAcao(p2, p1, inventario2);
            }

            turno++;
        }

        Console.WriteLine();
        Console.WriteLine("=== FIM DO COMBATE ===");

        if (p1.EstaVivo)
            Console.WriteLine($"{p1.Nome} venceu o combate!");
        else
            Console.WriteLine($"{p2.Nome} venceu o combate!");
    }

    static void FazerAcao(Personagem atacante, Personagem alvo, List<IUsavel> inventario)
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
                Console.WriteLine("  Habilidades:");
                for (int i = 0; i < atacante.Habilidades.Count; i++)
                {
                    Habilidade h = atacante.Habilidades[i];
                    Console.WriteLine($"  [{i + 1}] {h.Nome} (Dano:{h.Dano} Cura:{h.Cura})");
                }
                Console.Write("  Escolha: ");
                int hi = int.Parse(Console.ReadLine()) - 1;

                if (hi >= 0 && hi < atacante.Habilidades.Count)
                {
                    atacante.Habilidades[hi].Usar(atacante, alvo);
                    acaoFeita = true;
                }
                else Console.WriteLine("  Opcao invalida!");
            }
            else if (opcao == "3")
            {
                if (inventario.Count == 0)
                {
                    Console.WriteLine("  Inventario vazio!");
                }
                else
                {
                    Console.WriteLine("  Itens:");
                    for (int i = 0; i < inventario.Count; i++)
                        Console.WriteLine($"  [{i + 1}] {inventario[i].Nome}");

                    Console.Write("  Escolha: ");
                    int ii = int.Parse(Console.ReadLine()) - 1;

                    if (ii >= 0 && ii < inventario.Count)
                    {
                        inventario[ii].Usar(atacante);
                        inventario.RemoveAt(ii);
                        acaoFeita = true;
                    }
                    else Console.WriteLine("  Opcao invalida!");
                }
            }
            else Console.WriteLine("  Opcao invalida!");
        }
    }
}