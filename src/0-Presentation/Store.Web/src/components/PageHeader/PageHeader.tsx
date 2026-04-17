interface PageHeaderProps {
  title: string;
  subtitle?: string;
  showDivider?: boolean;
}

export default function PageHeader({
  title,
  subtitle,
  showDivider = true,
}: PageHeaderProps) {
  return (
    <div className="mb-4">
      <h3 className="mb-1">{title}</h3>

      {subtitle && (
        <small className="text-muted">{subtitle}</small>
      )}

      {showDivider && <hr />}
    </div>
  );
}