public interface HealthBarVisitor

{
	// this interface defines a visitor that children
	// of Movable use to update their healthbar
	void Visit(string TYPE, int Health, int MaxHealth);
}
